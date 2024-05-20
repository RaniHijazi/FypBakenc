using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;
using Fyp.Models;
using Fyp.Interfaces;

public class ChatHub : Hub
{
    private readonly IMessageRepository _messageRepository;
    private readonly IUserRepository _userRepository;
    private readonly DataContext _context;

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

        
        await Clients.Users(senderId.ToString(), recipientId.ToString()).SendAsync("ReceiveMessage", message);
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
        var userId = Context.UserIdentifier;
        if (userId != null)
        {
            var user = await _context.users.FindAsync(int.Parse(userId));
            if (user != null)
            {
                user.MemberStatus = "Online";
                await _context.SaveChangesAsync();
            }
        }

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        var userId = Context.UserIdentifier;
        if (userId != null)
        {
            var user = await _context.users.FindAsync(int.Parse(userId));
            if (user != null)
            {
                user.MemberStatus = "Offline";
                await _context.SaveChangesAsync();
            }
        }

        await base.OnDisconnectedAsync(exception);
    }
}
