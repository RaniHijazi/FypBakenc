using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fyp.Dto;
using Fyp.Models;
using Fyp.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Fyp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommunityController : ControllerBase
    {
        private readonly ICommunityRepository _repository;

        public CommunityController(ICommunityRepository repository)
        {
            _repository = repository;
        }

        [HttpPost("CreateCommunity")]
        public async Task<IActionResult> CreatePreCommunity(CommunityDto dto)
        {
            try
            {
                await _repository.CreateCommunity(dto);
                return Ok("Community created successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error creating community: {ex.Message}");
            }
        }

        [HttpPost("CreateSubCommunity")]
        public async Task<IActionResult> CreatePreSubCommunity([FromForm] SaveRequest request)
        {
            try
            {
                await _repository.CreateSubCommunity(request.preId, request.name, request.Description, request.Image);
                return Ok("Subcommunity created successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error creating subcommunity: {ex.Message}");
            }
        }

        [HttpPost("CreateAdminSubCommunity")]
        public async Task<IActionResult> CreateAdminSubCommunity([FromForm] CommunityRequest request)
        {
            try
            {
                await _repository.CreateAdminSubCommunity(request.Maincommunityname, request.name, request.Description, request.Image);
                return Ok("Subcommunity created successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error creating subcommunity: {ex.Message}");
            }
        }

        [HttpPost("AddUsersToSubCommunity/{subCommunityId}")]
        public async Task<IActionResult> AddUsersToSubCommunity([FromBody] List<string> userNames, int subCommunityId)
        {
            try
            {
                await _repository.AddUsersToSubCommunity(userNames, subCommunityId);
                return Ok();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("RemoveUsersFromSubCommunity/{subCommunityId}")]
        public async Task<IActionResult> RemoveUsersFromSubCommunity([FromBody] List<string> userNames, int subCommunityId)
        {
            try
            {
                await _repository.RemoveUsersFromSubCommunity(userNames, subCommunityId);
                return Ok();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("DeleteSubCommunity")]
        public async Task<IActionResult> DeleteSubCommunity(int subCommunityId)
        {
            try
            {
                await _repository.DeleteSubCommunity(subCommunityId);
                return Ok("Subcommunity deleted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error deleting subcommunity: {ex.Message}");
            }
        }

        [HttpGet("subCommunities")]
        public async Task<IActionResult> GetPreSubCommunities(int preCommunityId)
        {
            try
            {
                var subCommunities = await _repository.GetSubCommunities(preCommunityId);
                return Ok(subCommunities);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("adminSubCommunities")]
        public async Task<IActionResult> GetAdminSubCommunities()
        {
            try
            {
                var subCommunities = await _repository.GetAdminSubCommunities();
                return Ok(subCommunities);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{id}/toggle-status")]
        public async Task<IActionResult> ToggleCommunityStatus(int id)
        {
            var newStatus = await _repository.ToggleCommunityStatus(id);
            if (newStatus == null)
            {
                return NotFound("Community not found");
            }
            return Ok(newStatus);
        }

        [HttpPost("AddUserToSubCommunity")]
        public async Task<IActionResult> AddUserToSubCommunity([FromBody] AddUserToSubCommunityRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _repository.AddUserToSubCommunity(request.UserId, request.SubCommunityId);
                return Ok(new { message = "User successfully added to sub-community" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("RemoveUserFromSubCommunity")]
        public async Task<IActionResult> RemoveUserFromSubCommunity(int UserId,int SubCommunityId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _repository.RemoveUserFromSubCommunity(UserId,SubCommunityId);
                return Ok(new { message = "User successfully removed from sub-community" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("IsUserMemberOfSubCommunity")]
        public async Task<IActionResult> IsUserMemberOfSubCommunity([FromQuery] int userId, [FromQuery] int subCommunityId)
        {
            try
            {
                var isMember = await _repository.IsUserMemberOfSubCommunity(userId, subCommunityId);
                return Ok(new { isMember });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

    }
}

public class SaveRequest
{
    public IFormFile? Image { get; set; }
    public string name{ get; set; }
    public int preId { get; set; }
    public string Description { get; set; }

}

public class CommunityRequest
{
    public IFormFile? Image { get; set; }
    public string name { get; set; }
    public string Maincommunityname { get; set; }
    public string Description { get; set; }

}

public class AddUserToSubCommunityRequest
{
    public int UserId { get; set; }
    public int SubCommunityId { get; set; }
}