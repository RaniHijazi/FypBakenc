using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fyp.Dto;
using Fyp.Models;
using Fyp.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fyp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostController : ControllerBase
    {
        private readonly IPostRepository _repository;

        public PostController(IPostRepository repository)
        {
            _repository = repository;
        }

        [HttpPost("CreatePost")]
        public async Task<IActionResult> CreatePrePost(int user_id, int precommunity_id, PostDto dto)
        {
            try
            {
                await _repository.CreatePrePost(user_id, precommunity_id, dto);
                return Ok("Post created successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error creating post: {ex.Message}");
            }
        }

        [HttpPost("CreateSubPost")]
        public async Task<IActionResult> CreatePreSubPosts(int user_id, int precommunity_id, int presubcommunity_id, PostDto dto)
        {
            try
            {
                await _repository.CreatePreSubPosts(user_id, precommunity_id, presubcommunity_id, dto);
                return Ok("Subpost created successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error creating subpost: {ex.Message}");
            }
        }

        [HttpDelete("DeletePost")]
        public async Task<IActionResult> DeletePost(int post_id)
        {
            try
            {
                await _repository.DeletePost(post_id);
                return Ok("Post deleted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error deleting post: {ex.Message}");
            }
        }

        [HttpPost("LikePost")]
        public async Task<IActionResult> LikePost(int post_id, int user_id)
        {
            try
            {
                await _repository.LikePost(post_id, user_id);
                return Ok("Post liked successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error liking post: {ex.Message}");
            }
        }

        [HttpPost("DislikePost")]
        public async Task<IActionResult> DislikePost(int post_id, int user_id)
        {
            try
            {
                await _repository.DislikePost(post_id, user_id);
                return Ok("Post disliked successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error disliking post: {ex.Message}");
            }
        }

        [HttpPost("LikeComment")]
        public async Task<IActionResult> LikeComment(int comment_id, int user_id)
        {
            try
            {
                await _repository.LikeComment(comment_id, user_id);
                return Ok("Comment liked successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error liking comment: {ex.Message}");
            }
        }

        [HttpPost("DislikeComment")]
        public async Task<IActionResult> DislikeComment(int comment_id, int user_id)
        {
            try
            {
                await _repository.DislikeComment(comment_id, user_id);
                return Ok("Comment disliked successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error disliking comment: {ex.Message}");
            }
        }

        [HttpPost("CommentOnPost")]
        public async Task<IActionResult> CommentOnPost(int post_id, int user_id, CommentDto dto)
        {
            try
            {
                await _repository.CommentOnPost(post_id, user_id, dto);
                return Ok("Comment added successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error adding comment: {ex.Message}");
            }
        }

        [HttpDelete("DeleteComment")]
        public async Task<IActionResult> DeleteComment(int comment_id)
        {
            try
            {
                await _repository.DeleteComment(comment_id);
                return Ok("Comment deleted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error deleting comment: {ex.Message}");
            }
        }

        [HttpGet("PostLikesNb")]
        public async Task<IActionResult> PostLikesNb(int post_id)
        {
            try
            {
                var likesCount = await _repository.PostLikesNb(post_id);
                return Ok(likesCount);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error getting post likes count: {ex.Message}");
            }
        }

        [HttpGet("CommentLikesNb")]
        public async Task<IActionResult> CommentLikesNb(int comment_id)
        {
            try
            {
                var likesCount = await _repository.CommentLikesNb(comment_id);
                return Ok(likesCount);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error getting comment likes count: {ex.Message}");
            }
        }

        [HttpGet("PostCommentsNb")]
        public async Task<IActionResult> PostCommentsNb(int post_id)
        {
            try
            {
                var commentsCount = await _repository.PostCommentsNb(post_id);
                return Ok(commentsCount);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error getting post comments count: {ex.Message}");
            }
        }

        [HttpGet("AllCommentsWithDetails")]
        public async Task<IActionResult> GetAllCommentsWithDetails()
        {
            try
            {
                var comments = await _repository.GetAllCommentsWithDetails();
                return Ok(comments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error getting comments with details: {ex.Message}");
            }
        }

        [HttpGet("PrePosts")]
        public async Task<IActionResult> GetPrePosts(int PreCommunityId)
        {
            try
            {
                var posts = await _repository.GetPrePosts(PreCommunityId);
                return Ok(posts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error getting pre posts: {ex.Message}");
            }
        }

        [HttpGet("PreSubPosts")]
        public async Task<IActionResult> GetPreSubPosts(int PreCommunityId, int subCommunityId)
        {
            try
            {
                var posts = await _repository.GetPreSubPosts(PreCommunityId, subCommunityId);
                return Ok(posts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error getting pre sub posts: {ex.Message}");
            }
        }
    }
}
