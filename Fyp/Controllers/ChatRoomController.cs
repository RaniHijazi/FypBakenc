using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Fyp.Interfaces;
using Fyp.Models;
using Fyp.Dto;

namespace Fyp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatRoomController : ControllerBase
    {
        private readonly IChatRoomRepository _chatRoomRepository;
        private readonly IMessageRepository _messageRepository;

        public ChatRoomController(IChatRoomRepository chatRoomRepository, IMessageRepository messageRepository)
        {
            _chatRoomRepository = chatRoomRepository;
            _messageRepository = messageRepository;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateChatRoom([FromBody] ChatRoomDto dto, [FromQuery] int userId)
        {
            if (dto == null || string.IsNullOrEmpty(dto.RoomName))
            {
                return BadRequest("Invalid room data.");
            }

            try
            {
                var chatRoom = await _chatRoomRepository.CreateChatRoom(dto, userId);
                return Ok(chatRoom);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpPost("{roomId}/user/{userId}")]
        public async Task<IActionResult> AddUserToRoom(int roomId, int userId)
        {
            try
            {
                await _chatRoomRepository.AddUserToRoom(userId, roomId);
                return Ok($"User with ID {userId} added to room with ID {roomId}");
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to add user to room: {ex.Message}");
            }
        }


        [HttpDelete("{roomId}/user/{userId}")]
        public async Task<IActionResult> RemoveUserFromRoom(int roomId, int userId)
        {
            try
            {
                await _chatRoomRepository.RemoveUserFromRoom(userId, roomId);
                return Ok($"User with ID {userId} removed from room with ID {roomId}");
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to remove user from room: {ex.Message}");
            }
        }


        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserChatRooms(int userId)
        {
            try
            {
                var chatRooms = await _chatRoomRepository.GetUserChatRooms(userId);
                return Ok(chatRooms);
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to retrieve user's chat rooms: {ex.Message}");
            }
        }

        [HttpGet("{roomId}")]
        public async Task<IActionResult> GetChatRoomById(int roomId)
        {
            try
            {
                var chatRoom = await _chatRoomRepository.GetChatRoomById(roomId);
                if (chatRoom == null)
                    return NotFound($"Chat room with ID {roomId} not found");

                return Ok(chatRoom);
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to retrieve chat room: {ex.Message}");
            }
        }


        [HttpGet("{roomId}/messages")]
        public async Task<IActionResult> GetRoomMessages(int roomId)
        {
            try
            {
                var messages = await _chatRoomRepository.GetRoomMessages(roomId);
                return Ok(messages);
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to retrieve room messages: {ex.Message}");
            }
        }

        [HttpPost("createandadd")]
        public async Task<ActionResult<ChatRoom>> CreateChatRoomWithUsers([FromForm] CreateChatRoomRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.ChatRoomName) || request.UserIds == null || request.UserIds.Count == 0)
            {
                return BadRequest("Invalid request data.");
            }

            try
            {
                var chatRoom = await _chatRoomRepository.CreateChatRoomWithUsers(request.ChatRoomName, request.CreatorUserId, request.UserIds, request.Image);
                return Ok(chatRoom);
            }
            catch (Exception ex)
            {
               
                return StatusCode(500, "Internal server error");
            }
        }




    }
}

public class CreateChatRoomRequest
{
    public IFormFile? Image { get; set; }
    public string ChatRoomName { get; set; }
    public int CreatorUserId { get; set; }
    public List<int> UserIds { get; set; }
}