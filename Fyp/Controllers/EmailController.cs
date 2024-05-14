using Fyp.Dto;
using Fyp.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Fyp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IEmailRepository _emailRepository;

        public EmailController(IEmailRepository emailRepository)
        {
            _emailRepository = emailRepository;
        }


        [HttpPost("send-verification-code")]
        public IActionResult SendVerificationCode(string userEmail)
        {
            try
            {
                _emailRepository.SendVerificationCode(userEmail);
                return Ok("Verification code sent successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to send verification code: {ex.Message}");
            }
        }
    }
    
}