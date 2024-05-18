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
        public async Task<IActionResult> CreatePreSubCommunity(int preId, string name)
        {
            try
            {
                await _repository.CreateSubCommunity(preId, name);
                return Ok("Subcommunity created successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error creating subcommunity: {ex.Message}");
            }
        }

        [HttpPost("AddUserToSubCommunity")]
        public async Task<IActionResult> AddUserToPreSubCommunity(int userId, int presubCommunityId)
        {
            try
            {
                await _repository.AddUserToSubCommunity(userId, presubCommunityId);
                return Ok("User added to subcommunity successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error adding user to subcommunity: {ex.Message}");
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

        [HttpGet("{CommunityId}/subCommunities")]
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
    }
}
