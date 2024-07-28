using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Fyp.Interfaces;
using Fyp.Models;
using Microsoft.EntityFrameworkCore;
using Fyp.Dto;

namespace Fyp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MessageController : ControllerBase
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IUserRepository _userRepository;
        private readonly IChatRoomRepository _chatRoomRepository;
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly DataContext _context;

        public MessageController(IMessageRepository messageRepository, IUserRepository userRepository, IChatRoomRepository chatRoomRepository, IHubContext<ChatHub> hubContext, DataContext context)
        {
            _messageRepository = messageRepository;
            _userRepository = userRepository;
            _chatRoomRepository = chatRoomRepository;
            _hubContext = hubContext;
            _context = context;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendMessageToUser(int senderId, int recipientId, string messageContent)
        {
            try
            {
                var sender = await _userRepository.GetUserByIdAsync(senderId);
                var recipient = await _userRepository.GetUserByIdAsync(recipientId);

                if (sender == null || recipient == null)
                {
                    return BadRequest("Sender or recipient not found.");
                }

                var message = new Message
                {
                    SenderId = senderId,
                    RecipientId = recipientId,
                    Content = messageContent,
                    Timestamp = DateTime.UtcNow
                };

                await _messageRepository.AddMessage(message);
                await _hubContext.Clients.Users(senderId.ToString(), recipientId.ToString()).SendAsync("ReceiveMessage", message.Content, message.SenderId, message.RecipientId, message.Timestamp);
                Console.WriteLine($"SendMessageToUser: Sent message to {recipientId} with content: {messageContent}");

                return Ok(message);
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to send message: {ex.Message}");
            }
        }


        [HttpPost("sendToRoom")]
        public async Task<IActionResult> SendMessageToRoom(int senderId, int roomId, string messageContent)
        {
            try
            {
                var sender = await _userRepository.GetUserByIdAsync(senderId);
                var room = await _context.chat_rooms.FindAsync(roomId);

                if (sender == null || room == null)
                {
                    return BadRequest("Sender or room not found.");
                }

               
                var userInRoom = await _context.user_chat_rooms
                    .AnyAsync(ur => ur.UserId == senderId && ur.RoomId == roomId);

                if (!userInRoom)
                {
                    return BadRequest("You are not in the room.");
                }

                var message = new Message
                {
                    SenderId = senderId,
                    RoomId = roomId,
                    Content = messageContent,
                    Timestamp = DateTime.UtcNow
                };

                await _messageRepository.AddMessage(message);

                await _hubContext.Clients.Group(roomId.ToString()).SendAsync("ReceiveRoomMessage", message);

                return Ok(message);
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to send message to room: {ex.Message}");
            }
        }

        [HttpPost("leaveRoom")]
        public async Task<IActionResult> LeaveRoom(int userId, int roomId)
        {
            try
            {
                var userRoom = await _context.user_chat_rooms
                    .FirstOrDefaultAsync(ur => ur.UserId == userId && ur.RoomId == roomId);

                if (userRoom == null)
                {
                    return NotFound("User is not a member of the room.");
                }

                _context.user_chat_rooms.Remove(userRoom);
                await _context.SaveChangesAsync();

                
                await _hubContext.Clients.Group(roomId.ToString()).SendAsync("UserLeftRoom", userId);

                return Ok("User has left the room.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to leave the room: {ex.Message}");
            }
        }

        [HttpGet("sender/{senderId}")]
        public async Task<IActionResult> GetMessagesBySenderId(int senderId,int receiverId)
        {
            try
            {
                var messages = await _messageRepository.GetMessagesBySenderId(senderId,receiverId);
                return Ok(messages);
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to retrieve messages: {ex.Message}");
            }
        }

        

        [HttpGet("room/{roomId}")]
        public async Task<IActionResult> GetRoomMessages(int roomId)
        {
            try
            {
                var messages = await _messageRepository.GetRoomMessages(roomId);
                return Ok(messages);
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to retrieve room messages: {ex.Message}");
            }
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserMessages(int userId)
        {
            var messages = await _context.messages
                .Where(m => m.SenderId == userId || m.RecipientId == userId)
                .OrderBy(m => m.Timestamp)
                .Select(m => new MessageDto2
                {
                    MessageId = m.MessageId,
                    SenderId = m.SenderId,
                    RecipientId = m.RecipientId,
                    RoomId = m.RoomId,
                    Content = m.Content,
                    Timestamp = m.Timestamp,
                    RoomName = m.Room != null ? m.Room.RoomName : null,
                    ProfilePath = m.Room != null ? m.Room.ProfilePath : null 
                })
                .ToListAsync();

            return Ok(messages);
        }



        [HttpPost("replyToStory")]
        public async Task<IActionResult> ReplyToStory([FromBody] ReplyToStoryRequest request)
        {
            try
            {
                var message = new Message
                {
                    SenderId = request.SenderId,
                    RecipientId = request.ReceiverId,
                    StoryId = request.StoryId,
                    Content = request.Content,
                    Timestamp = DateTime.UtcNow
                };

                _context.messages.Add(message);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Reply sent successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

    }
}
public class ReplyToStoryRequest
{
    public int SenderId { get; set; }
    public int ReceiverId { get; set; }
    public int StoryId { get; set; }
    public string Content { get; set; }
}