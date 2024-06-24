using Fyp.Dto;
using Fyp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IMessageRepository
{
    Task<Message> AddMessage(Message message);
    Task<List<MessageDto>> GetMessagesBySenderId(int senderId, int recipientId);
    Task<List<MessageDto>> GetRoomMessages(int roomId);
}