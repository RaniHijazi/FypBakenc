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

    public async Task SendMessageToUser(int senderId, int recipientId, string messageContent)
    {
        Console.WriteLine($"SendMessageToUser: senderId={senderId}, recipientId={recipientId}, messageContent={messageContent}");

        var sender = await _userRepository.GetUserByIdAsync(senderId);
        var recipient = await _userRepository.GetUserByIdAsync(recipientId);

        if (sender == null || recipient == null)
        {
            Console.WriteLine("SendMessageToUser: Sender or recipient not found.");
            throw new ArgumentException("Sender or recipient not found.");
        }

        var message = new Message
        {
            SenderId = senderId,
            RecipientId = recipientId,
            Content = messageContent,
            Timestamp = DateTime.UtcNow
        };

        await _messageRepository.AddMessage(message);
        await Clients.Users(senderId.ToString(), recipientId.ToString()).SendAsync("ReceiveMessage", message.Content, message.SenderId, message.RecipientId, message.Timestamp);
        Console.WriteLine($"SendMessageToUser: Sent message to {recipientId} with content: {messageContent}");
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
            ConnectedUsers[Context.ConnectionId] = userId;
            Console.WriteLine($"OnConnectedAsync: User {userId} connected with connection ID {Context.ConnectionId}");
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

        await base.OnDisconnectedAsync(exception);
    }

    public static string[] GetConnectedUsers()
    {
        var users = ConnectedUsers.Values.Distinct().ToArray();
        Console.WriteLine($"GetConnectedUsers: {string.Join(", ", users)}");
        return users;
    }
}
