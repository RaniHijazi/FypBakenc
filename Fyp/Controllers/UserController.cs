using Fyp.Dto;
using Fyp.Interfaces;
using Fyp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Fyp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly BlobStorageService _blobStorageService;
        private readonly DataContext _context;

        public UserController(IUserRepository userRepository, IConfiguration configuration, BlobStorageService blobStorageService, DataContext context)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _blobStorageService = blobStorageService;
            _context = context;
        }

        private async Task<string> CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value));

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: cred
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return await Task.FromResult(jwt);
        }

        [HttpPost("VerifyVerificationCode")]
        public async Task<IActionResult> VerifyVerificationCode([FromBody] EmailDto dto)
        {
            try
            {
                bool isValid = await _userRepository.VerifyVerificationCode(dto.UserEmail, dto.VerificationCode);
                return Ok(isValid);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] SignUpDto signUpDto)
        {
            try
            {
                var newUser = await _userRepository.SignUp(signUpDto);
                var token = await CreateToken(newUser);
                return Ok(new { newUser, token });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(500, "An error occurred while processing the request");
            }
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<User>> GetUserById(int userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }





        [HttpPost("signin")]
        public async Task<IActionResult> SignIn([FromBody] SignInDto signInDto)
        {
            try
            {
                var user = await _userRepository.SignIn(signInDto);
                var token = await CreateToken(user);
                return Ok(new { user, token });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An unexpected error occurred" });
            }
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _userRepository.GetAllUsers();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("follow")]
        public async Task<IActionResult> Follow([FromBody] FollowRequest request)
        {
            await _userRepository.FollowUserAsync(request.FollowerId, request.FollowedId);
            return Ok();
        }

        [HttpPost("unfollow")]
        public async Task<IActionResult> Unfollow([FromBody] FollowRequest request)
        {
            await _userRepository.UnfollowUserAsync(request.FollowerId, request.FollowedId);
            return Ok();
        }

        [HttpPost("uploadimage")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadImage([FromForm] IFormFile image)
        {
            try
            {
                if (image == null || image.Length == 0)
                    return BadRequest("No image uploaded");

                string imageUrl = await _blobStorageService.UploadImageAsync(image);
                return Ok(new { ImageUrl = imageUrl });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to upload image: {ex.Message}");
            }
        }

        [HttpPost("saveurl")]
        public async Task<IActionResult> SaveUrl([FromForm] SaveUrlRequest request)
        {
            try
            {
                await _blobStorageService.SaveUserUrlAsync(request.UserId, request.Image);
                return Ok("User profile image URL saved successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to save user profile image URL: {ex.Message}");
            }
        }
        [HttpGet("{userId}/profile")]
        public async Task<ActionResult<UserProfileDto>> GetUserProfile(int userId)
        {
            try
            {
                var userProfile = await _userRepository.GetUserProfile(userId);
                return Ok(userProfile);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error: " + ex.Message });
            }
        }

        [HttpGet("{followerId}/isFollowing/{followedId}")]
        public async Task<IActionResult> IsFollowing(int followerId, int followedId)
        {
            try
            {
                var isFollowing = await _userRepository.IsFollowingAsync(followerId, followedId);
                return Ok(isFollowing);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpPost("uploadStory")]
        public async Task<IActionResult> UploadStory([FromForm] SaveUrlStoryRequest request)
        {
            try
            {
                var story = new Story
                {
                    UserId = request.UserId,
                    StoryPath = await _blobStorageService.UploadStoryAsync(request.Story),
                    CreatedAt = DateTime.UtcNow
                };

                _context.stories.Add(story);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Story uploaded successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("{userId}/getFollowingStories")]
        public async Task<IActionResult> GetFollowingStories(int userId)
        {
            try
            {
                var stories = await _userRepository.GetStoriesAsync(userId);  
                return Ok(stories);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("{userId}/following")]
        public async Task<IActionResult> GetFollowingUsers(int userId)
        {
            try
            {
                var followingUsers = await _userRepository.GetFollowingUsersAsync(userId);
                return Ok(followingUsers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }


    }
    public class SaveUrlRequest
    {
        public int UserId { get; set; }
        public IFormFile Image { get; set; }
       
    }
    public class SaveUrlStoryRequest
    {
        public int UserId { get; set; }
        public IFormFile Story { get; set; }
    }


    public class FollowRequest
    {
        public int FollowerId { get; set; }
        public int FollowedId { get; set; }
    }
}
