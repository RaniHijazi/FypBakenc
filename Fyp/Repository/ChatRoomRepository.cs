using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Fyp.Interfaces;
using Fyp.Models;
using Fyp.Dto;

public class ChatRoomRepository : IChatRoomRepository
{
    private readonly DataContext _context;

    public ChatRoomRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<ChatRoom> CreateChatRoom(ChatRoomDto dto, int userId)
    {
        var room = new ChatRoom
        {
            RoomName = dto.RoomName,
            Description = dto.Description,
            nbMembers = 1 
        };

        _context.chat_rooms.Add(room);
        await _context.SaveChangesAsync();

        var userRoom = new UserChatRoom
        {
            UserId = userId,
            RoomId = room.Id
        };

        _context.user_chat_rooms.Add(userRoom);
        await _context.SaveChangesAsync();

        return room;
    }


    public async Task AddUserToRoom(int userId, int roomId)
    {
        var room = await _context.chat_rooms.FindAsync(roomId);
        if (room == null)
        {
            throw new InvalidOperationException("Didn't find room");
        }
        room.nbMembers += 1;
        var userRoom = new UserChatRoom { UserId = userId, RoomId = roomId };

        _context.user_chat_rooms.Add(userRoom);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveUserFromRoom(int userId, int roomId)
    {
        var userRoom = await _context.user_chat_rooms.FirstOrDefaultAsync(ucr => ucr.UserId == userId && ucr.RoomId == roomId);
        if (userRoom != null)
        {
            _context.user_chat_rooms.Remove(userRoom);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<List<ChatRoomDto2>> GetUserChatRooms(int userId)
    {
        var userChatRooms = await _context.user_chat_rooms
            .Where(ucr => ucr.UserId == userId)
            .Include(ucr => ucr.Room) 
            .ToListAsync();

        var chatRoomDtos = userChatRooms.Select(ucr => new ChatRoomDto2
        {
            RoomName = ucr.Room.RoomName,
            Description = ucr.Room.Description,
            NbMembers = ucr.Room.UserChatRooms.Count 
        }).ToList();

        return chatRoomDtos;
    }

    public async Task<ChatRoomDto> GetChatRoomById(int roomId)
    {
        var chatRoom = await _context.chat_rooms.FindAsync(roomId);

        if (chatRoom == null)
        {
            throw new InvalidOperationException("room not found");
        }

        return new ChatRoomDto
        {
            RoomName = chatRoom.RoomName,
            Description = chatRoom.Description
            
        };
    }

    public async Task<List<Message>> GetRoomMessages(int roomId)
    {
        return await _context.messages
            .Where(m => m.RoomId == roomId)
            .OrderBy(m => m.Timestamp) 
            .ToListAsync();
    }

}
