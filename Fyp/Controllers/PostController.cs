﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fyp.Dto;
using Fyp.Models;
using Fyp.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Fyp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostController : ControllerBase
    {
        private readonly IPostRepository _repository;
        private readonly IHubContext<ChatHub> _hubContext;

        public PostController(IPostRepository repository, IHubContext<ChatHub> hubContext)
        {
            _repository = repository;
            _hubContext = hubContext;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreatePost([FromForm] SaveUrlRequest2 request2)
        {
            try
            {
                await _repository.CreatePrePost(request2.UserId, request2.CommunityId, request2.Description, request2.Image);
                return Ok(new { message = "Post created successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }



        [HttpPost("CreateSubPost")]
        public async Task<IActionResult> CreatePreSubPosts([FromForm] SaveRequest2 request)
        {
            try
            {
                await _repository.CreatePreSubPosts(request.UserId, request.CommunityId, request.presubcommunity_id, request.Description, request.Image);
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

        [HttpGet("post/{postId}")]
        public async Task<ActionResult<IEnumerable<GetCommentDto>>> GetCommentsForPost(int postId)
        {
            try
            {
                var comments = await _repository.GetAllCommentsWithDetails(postId);
                return Ok(comments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("PrePosts")]
        public async Task<IActionResult> GetPrePosts(int PreCommunityId, int currentUserId)
        {
            try
            {
                var posts = await _repository.GetPrePosts(PreCommunityId,currentUserId);
                return Ok(posts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error getting pre posts: {ex.Message}");
            }
        }

        [HttpGet("{postId}/hasLiked")]
        public async Task<IActionResult> HasUserLikedPost(int postId, [FromQuery] int userId)
        {
            try
            {
                bool hasLiked = await _repository.HasUserLikedPost(postId, userId);
                return Ok(hasLiked);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{commentId}/hasCommented")]
        public async Task<IActionResult> HasUserLikedComment(int commentId, [FromQuery] int userId)
        {
            try
            {
                bool hasCommented = await _repository.HasUserLikedComment(commentId, userId);
                return Ok(hasCommented);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("PreSubPosts")]
        public async Task<IActionResult> GetPreSubPosts( int subCommunityId)
        {
            try
            {
                var posts = await _repository.GetPreSubPosts( subCommunityId);
                return Ok(posts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error getting pre sub posts: {ex.Message}");
            }
        }

        [HttpGet("subcommunity-posts")]
        public async Task<ActionResult<List<GetPostDto>>> GetPostsOfAllSubcommunities([FromQuery] int preCommunityId)
        {
            try
            {
                var posts = await _repository.GetPostsOfAllSubcommunities(preCommunityId);
                return Ok(posts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<List<GetPostDto>>> GetUserPosts(int userId)
        {
            try
            {
                var posts = await _repository.GetUserPosts(userId);
                if (posts == null || posts.Count == 0)
                {
                    return NotFound("No posts found for the specified user.");
                }
                return Ok(posts);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<List<Notification>>> GetNotifications(int userId)
        {
            try
            {
                var notifications = await _repository.GetAllNotificationsForUser(userId);
                return Ok(notifications);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("sendTestNotification")]
        public async Task<IActionResult> SendTestNotification([FromQuery] int userId, [FromQuery] string messageContent)
        {
            var connectedUsers = ChatHub.GetConnectedUsers();
            Console.WriteLine($"Connected users: {string.Join(", ", connectedUsers)}");

            var connectionIds = ChatHub.ConnectedUsers.Where(kvp => kvp.Value == userId.ToString()).Select(kvp => kvp.Key).ToList();
            if (connectionIds.Count > 0)
            {
                foreach (var connectionId in connectionIds)
                {
                    await _hubContext.Clients.Client(connectionId).SendAsync("ReceiveNotification", messageContent);
                }
                return Ok($"Test notification sent to user {userId} with content: {messageContent}");
            }
            else
            {
                return NotFound($"User {userId} is not connected.");
            }
        }

    }
}
public class SaveUrlRequest2
{  
    public IFormFile? Image { get; set; }
    public int CommunityId { get; set; }
    public int UserId { get; set; }
    public string? Description { get; set; }

}

public class SaveRequest2
{
    public IFormFile? Image { get; set; }
    public int CommunityId { get; set; }
    public int presubcommunity_id { get; set; }
    
    public int UserId { get; set; }
    public string? Description { get; set; }

}