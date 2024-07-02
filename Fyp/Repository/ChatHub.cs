using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;
using Fyp.Models;
using Fyp.Interfaces;
using System.Collections.Concurrent;

public class ChatHub : Hub
{
    private readonly IMessageRepository _messageRepository;
    private readonly IUserRepository _userRepository;
    private readonly DataContext _context;
    private static ConcurrentDictionary<string, string> connectedUsers = new ConcurrentDictionary<string, string>();
    public ChatHub(IMessageRepository messageRepository, IUserRepository userRepository, DataContext context)
    {
        _messageRepository = messageRepository;
        _userRepository = userRepository;
        _context = context;
    }

    public async Task SendMessageToUser(int senderId, int recipientId, string messageContent)
    {
        var sender = await _userRepository.GetUserByIdAsync(senderId);
        var recipient = await _userRepository.GetUserByIdAsync(recipientId);

        if (sender == null || recipient == null)
        {
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
    public async Task SendMessageToRoom(int senderId, int roomId, string messageContent)
    {
        var sender = await _userRepository.GetUserByIdAsync(senderId);
        var room = await _context.chat_rooms.FindAsync(roomId);

        if (sender == null || room == null)
        {
            throw new ArgumentException("Sender or room not found.");
        }

        var message = new Message
        {
            SenderId = senderId,
            RoomId = roomId,
            Content = messageContent,
            Timestamp = DateTime.UtcNow
        };

        await _messageRepository.AddMessage(message);
        await Clients.Group(roomId.ToString()).SendAsync("ReceiveRoomMessage", message);
    }

   public override async Task OnConnectedAsync()
    {
        var userId = Context.GetHttpContext().Request.Query["userId"];
        if (!string.IsNullOrEmpty(userId))
        {
            connectedUsers.TryAdd(Context.ConnectionId, userId);
            Console.WriteLine($"User connected: {userId}");
        }
        else
        {
            Console.WriteLine("User identifier is null or empty.");
        }

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        if (connectedUsers.TryRemove(Context.ConnectionId, out var userId))
        {
            Console.WriteLine($"User disconnected: {userId}");
        }
        else
        {
            Console.WriteLine("User connection ID not found in connected users list.");
        }

        await base.OnDisconnectedAsync(exception);
    }


    public Task<string[]> GetConnectedUsers()
    {
        // Return the list of connected user IDs
        return Task.FromResult(connectedUsers.Values.ToArray());
    }
}
