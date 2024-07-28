using Fyp.Interfaces;
using Fyp.Models;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;

public class ChatHub : Hub
{
    private readonly IMessageRepository _messageRepository;
    private readonly IUserRepository _userRepository;
    private readonly DataContext _context;
    public static ConcurrentDictionary<string, string> ConnectedUsers = new ConcurrentDictionary<string, string>();

    public ChatHub(IMessageRepository messageRepository, IUserRepository userRepository, DataContext context)
    {
        _messageRepository = messageRepository;
        _userRepository = userRepository;
        _context = context;
    }

    public async Task SendMessage(int senderId, int recipientId, string messageContent)
    {
        Console.WriteLine($"SendMessage: senderId={senderId}, recipientId={recipientId}, messageContent={messageContent}");

        var sender = await _userRepository.GetUserByIdAsync(senderId);
        var recipient = await _userRepository.GetUserByIdAsync(recipientId);

        if (sender == null || recipient == null)
        {
            Console.WriteLine("SendMessage: Sender or recipient not found.");
            throw new ArgumentException("Sender or recipient not found.");
        }

        var senderConnectionIds = ConnectedUsers.Where(kvp => kvp.Value == senderId.ToString()).Select(kvp => kvp.Key).ToList();
        var recipientConnectionIds = ConnectedUsers.Where(kvp => kvp.Value == recipientId.ToString()).Select(kvp => kvp.Key).ToList();

        if (recipientConnectionIds.Count > 0)
        {
            foreach (var connectionId in recipientConnectionIds)
            {
                Console.WriteLine($"SendMessage: Sending message to recipient connection ID {connectionId}.");
                await Clients.Client(connectionId).SendAsync("ReceiveMessage", senderId, recipientId, messageContent, DateTime.UtcNow);
            }
            Console.WriteLine($"SendMessage: Sent message from {senderId} to {recipientId} with content: {messageContent}");
        }
        else
        {
            Console.WriteLine($"SendMessage: Recipient {recipientId} is not connected.");
        }

        // Optionally, send the message to the sender's own connection as well
        if (senderConnectionIds.Count > 0)
        {
            foreach (var connectionId in senderConnectionIds)
            {
                Console.WriteLine($"SendMessage: Sending message confirmation to sender connection ID {connectionId}.");
                await Clients.Client(connectionId).SendAsync("ReceiveMessage", senderId, recipientId, messageContent, DateTime.UtcNow);
            }
        }

        // Save the message to the database
        var message = new Message
        {
            SenderId = senderId,
            RecipientId = recipientId,
            Content = messageContent,
            Timestamp = DateTime.UtcNow
        };

        await _messageRepository.AddMessage(message);
        Console.WriteLine($"SendMessage: Message saved to database from {senderId} to {recipientId}.");
    }



    public async Task SendNotification(int recipientId, string messageContent)
    {
        Console.WriteLine($"SendNotification: recipientId={recipientId}, messageContent={messageContent}");

        var recipient = await _userRepository.GetUserByIdAsync(recipientId);

        if (recipient == null)
        {
            Console.WriteLine("SendNotification: Recipient not found.");
            throw new ArgumentException("Recipient not found.");
        }

        var connectionIds = ConnectedUsers.Where(kvp => kvp.Value == recipientId.ToString()).Select(kvp => kvp.Key).ToList();
        if (connectionIds.Count > 0)
        {
            foreach (var connectionId in connectionIds)
            {
                Console.WriteLine($"SendNotification: Sending notification to connection ID {connectionId}.");
                await Clients.Client(connectionId).SendAsync("ReceiveNotification", messageContent);
            }
            Console.WriteLine($"SendNotification: Sent notification to {recipientId} with content: {messageContent}");
        }
        else
        {
            Console.WriteLine($"SendNotification: User {recipientId} is not connected.");
        }
    }

    public override async Task OnConnectedAsync()
    {
        var userId = Context.GetHttpContext().Request.Query["userId"];
        if (!string.IsNullOrEmpty(userId))
        {
            // Remove existing connections for the same userId
            var existingConnections = ConnectedUsers.Where(kvp => kvp.Value == userId).Select(kvp => kvp.Key).ToList();
            foreach (var connectionId in existingConnections)
            {
                ConnectedUsers.TryRemove(connectionId, out _);
                Console.WriteLine($"OnConnectedAsync: Removed existing connection ID {connectionId} for user {userId}");
            }

            // Add the new connection
            ConnectedUsers[Context.ConnectionId] = userId;
            Console.WriteLine($"OnConnectedAsync: User {userId} connected with connection ID {Context.ConnectionId}");

            // Print connected users
            PrintConnectedUsers();
        }
        else
        {
            Console.WriteLine("OnConnectedAsync: User ID not found in query string.");
        }

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        if (ConnectedUsers.TryRemove(Context.ConnectionId, out var userId))
        {
            Console.WriteLine($"OnDisconnectedAsync: User {userId} disconnected.");
        }
        else
        {
            Console.WriteLine("OnDisconnectedAsync: User connection ID not found in connected users list.");
        }

        // Print connected users
        PrintConnectedUsers();

        await base.OnDisconnectedAsync(exception);
    }

    public static string[] GetConnectedUsers()
    {
        var users = ConnectedUsers.Values.Distinct().ToArray();
        Console.WriteLine($"GetConnectedUsers: {string.Join(", ", users)}");
        return users;
    }

    public async Task Ping()
    {
        Console.WriteLine("Ping method called");
        await Clients.Caller.SendAsync("Pong", "Ping received successfully");
    }


    private void PrintConnectedUsers()
    {
        var connectedUsers = ConnectedUsers.Values.Distinct().ToList();
        Console.WriteLine("Currently connected users: ");
        foreach (var user in connectedUsers)
        {
            Console.WriteLine($"User ID: {user}");
        }
    }
}
