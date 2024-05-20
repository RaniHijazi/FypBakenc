using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Fyp.Interfaces;
using Fyp.Models;
using Fyp.Dto;

public class MessageRepository : IMessageRepository
{
    private readonly DataContext _context;

    public MessageRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<Message> AddMessage(Message message)
    {
        _context.messages.Add(message);
        await _context.SaveChangesAsync();
        return message;
    }

    public async Task<List<MessageDto>> GetMessagesBySenderId(int senderId)
    {
        return await _context.messages
            .Where(m => m.SenderId == senderId)
            .OrderBy(m => m.Timestamp)
            .Select(m => new MessageDto
            {
                Content = m.Content,
                SenderName = m.Sender.FullName,
                SenderProfilePath = m.Sender.ProfilePath,
                Timestamp = m.Timestamp
            })
            .ToListAsync();
    }

    public async Task<List<MessageDto>> GetMessagesByRecipientId(int recipientId)
    {
        return await _context.messages
            .Where(m => m.RecipientId == recipientId)
            .OrderBy(m => m.Timestamp)
            .Select(m => new MessageDto
            {
                Content = m.Content,
                SenderName = m.Sender.FullName,
                SenderProfilePath = m.Sender.ProfilePath,
                Timestamp = m.Timestamp
            })
            .ToListAsync();
    }
    
    public async Task<List<MessageDto>> GetRoomMessages(int roomId)
    {
        return await _context.messages
            .Where(m => m.RoomId == roomId)
            .OrderBy(m => m.Timestamp)
            .Select(m => new MessageDto
            {
                Content = m.Content,
                SenderName = m.Sender.FullName,
                SenderProfilePath = m.Sender.ProfilePath,
                Timestamp = m.Timestamp
            })
            .ToListAsync();
    }
   
}
