using Fyp.Dto;
using Fyp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IChatRoomRepository
{
    Task<ChatRoom> CreateChatRoom(ChatRoomDto dto, int userId);
    Task AddUserToRoom(int userId, int roomId);
    Task RemoveUserFromRoom(int userId, int roomId);
    Task<List<ChatRoomDto2>> GetUserChatRooms(int userId);
    Task<ChatRoomDto> GetChatRoomById(int roomId);
    Task<List<Message>> GetRoomMessages(int roomId);
    Task<ChatRoom> CreateChatRoomWithUsers(string chatRoomName, int creatorUserId, List<int> userIds, IFormFile? image);
}