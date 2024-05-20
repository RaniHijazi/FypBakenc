using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Fyp.Interfaces;
using Fyp.Models;

namespace Fyp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MessageController : ControllerBase
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IChatRoomRepository _chatRoomRepository;

        public MessageController(IMessageRepository messageRepository, IChatRoomRepository chatRoomRepository)
        {
            _messageRepository = messageRepository;
            _chatRoomRepository = chatRoomRepository;
        }

        
        [HttpPost]
        public async Task<IActionResult> SendMessage(Message message)
        {
            try
            {
                var addedMessage = await _messageRepository.AddMessage(message);
                return Ok(addedMessage);
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to send message: {ex.Message}");
            }
        }

       
        [HttpGet("sender/{senderId}")]
        public async Task<IActionResult> GetMessagesBySenderId(int senderId)
        {
            try
            {
                var messages = await _messageRepository.GetMessagesBySenderId(senderId);
                return Ok(messages);
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to retrieve messages: {ex.Message}");
            }
        }

        
        [HttpGet("recipient/{recipientId}")]
        public async Task<IActionResult> GetMessagesByRecipientId(int recipientId)
        {
            try
            {
                var messages = await _messageRepository.GetMessagesByRecipientId(recipientId);
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

       

    }
}
