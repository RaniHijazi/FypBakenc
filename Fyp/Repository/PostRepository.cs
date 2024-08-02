﻿using Fyp.Dto;
using Fyp.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Fyp.Repository
{
    public class PostRepository:IPostRepository
    {
        private readonly DataContext _context;
        private readonly BlobStorageService _blobStorageService;
        private readonly IHubContext<ChatHub> _hubContext;

        public PostRepository(DataContext context, BlobStorageService blobStorageService, IHubContext<ChatHub> hubContext)
        {
            _context = context;
            _blobStorageService = blobStorageService;
            _hubContext = hubContext;
        }

        public async Task CreatePrePost(int user_id, int precommunity_id, string? Description, IFormFile? image)
        {
            var user = await _context.users.FindAsync(user_id);
            var precommunityId = await _context.communities.FindAsync(precommunity_id);

            if (user == null)
            {
                throw new InvalidOperationException("User not found");
            }

            if (precommunityId == null)
            {
                throw new InvalidOperationException("Community not found");
            }

            string imageUrl = null;

            if (image != null)
            {
                imageUrl = await _blobStorageService.UploadImageAsync(image);
            }

            var prepost = new Post
            {
                Description = Description,
                ImageUrl = imageUrl,
                LikesCount = 0,
                CommentsCount = 0,
                ShareCount = 0,
                Timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                UserId = user_id,
                CommunityId = precommunity_id
            };

            _context.posts.Add(prepost);

            if (user.LastPostPointsAwarded != DateTime.Today)
            {
                var today = DateTime.Today;
                var userPosts = await _context.posts
                    .Where(l => l.UserId == user_id)
                    .ToListAsync();

                var postsCount = userPosts
                    .Count(l => DateTime.TryParse(l.Timestamp, out DateTime parsedDate) && parsedDate >= today);

                if (postsCount >= 1)
                {
                    user.LastPostPointsAwarded = DateTime.Today;
                    user.points += 20;

                    if (user.points >= 1000)
                    {
                        user.Level += 1;
                        user.points = 0;
                    }

                    _context.users.Update(user);
                }
            }

            await _context.SaveChangesAsync();
        }


        public async Task CreatePreSubPosts(int user_id, int precommunity_id, int presubcommunity_id, string? Description, IFormFile? image)
        {
            var user = await _context.users.FindAsync(user_id);
            var preCommunity = await _context.communities.FindAsync(precommunity_id);
            var preSubCommunity = await _context.sub_communities.FindAsync(presubcommunity_id);

            if (user == null)
            {
                throw new InvalidOperationException("User not found");
            }

            if (preCommunity == null)
            {
                throw new InvalidOperationException("Community not found");
            }

            if (preSubCommunity == null)
            {
                throw new InvalidOperationException("Subcommunity not found");
            }
            string imageUrl = null;

            if (image != null)
            {

                imageUrl = await _blobStorageService.UploadImageAsync(image);
            }

            var prepost = new Post
            {
                Description = Description,
                ImageUrl = imageUrl,
                LikesCount = 0,
                CommentsCount = 0,
                ShareCount = 0,
                Timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                UserId = user_id,
                CommunityId = precommunity_id,
                SubCommunityId = presubcommunity_id
            };

            _context.posts.Add(prepost);
            if (user.LastPostPointsAwarded != DateTime.Today)
            {
                var today = DateTime.Today;
                var userPosts = await _context.posts
                    .Where(l => l.UserId == user_id)
                    .ToListAsync();

                var postsCount = userPosts
                    .Count(l => DateTime.TryParse(l.Timestamp, out DateTime parsedDate) && parsedDate >= today);

                if (postsCount >= 5)
                {
                    user.LastPostPointsAwarded = DateTime.Today;
                    user.points += 20;

                    if (user.points >= 1000)
                    {
                        user.Level += 1;
                        user.points = 0;
                    }

                    _context.users.Update(user);
                }
            }
            await _context.SaveChangesAsync();
        }

        public async Task DeletePost(int post_id)
        {
            var post=await _context.posts.FindAsync(post_id);
            if (post == null)
            {
                throw new InvalidOperationException("Post isn't found");  
            }
            _context.posts.Remove(post);
                await _context.SaveChangesAsync();
        }
        private static string CalculateTimeAgo(DateTime timestamp)
        {
            TimeSpan timeDifference = DateTime.UtcNow - timestamp;

            if (timeDifference.TotalSeconds < 60)
            {
                return "just now";
            }
            else if (timeDifference.TotalMinutes < 60)
            {
                return $"{((int)timeDifference.TotalMinutes)} minute{((int)timeDifference.TotalMinutes != 1 ? "s" : "")} ago";
            }
            else if (timeDifference.TotalHours < 24)
            {
                return $"{((int)timeDifference.TotalHours)} hour{((int)timeDifference.TotalHours != 1 ? "s" : "")} ago";
            }
            else
            {
                return $"{((int)timeDifference.TotalDays)} day{((int)timeDifference.TotalDays != 1 ? "s" : "")} ago";
            }
        }

        public async Task<List<GetPostDto>> GetPrePosts(int PreCommunityId)
        {
            var posts = await _context.posts
                .Where(p => p.CommunityId == PreCommunityId)
                .Select(p => new GetPostDto
                {
                    Id=p.Id,
                    Description = p.Description,
                    ImageUrl = p.ImageUrl,
                    LikesCount = p.LikesCount,
                    CommentsCount = p.CommentsCount,
                    ShareCount = p.ShareCount,
                    Timestamp = CalculateTimeAgo(DateTime.Parse(p.Timestamp)),
                    UserFullName = p.User.FullName,
                    UserProfileImageUrl = p.User.ProfilePath,
                    UserId=p.UserId

                })
                .ToListAsync();

            return posts;
        }

        public async Task<List<GetPostDto>> GetPreSubPosts( int subCommunityId)
        {
            var posts = await _context.posts
                .Where(p =>p.SubCommunityId == subCommunityId)
                .Select(p => new GetPostDto
                {
                    Id = p.Id,
                    Description = p.Description,
                    ImageUrl = p.ImageUrl,
                    LikesCount = p.LikesCount,
                    CommentsCount = p.CommentsCount,
                    ShareCount = p.ShareCount,
                    Timestamp = CalculateTimeAgo(DateTime.Parse(p.Timestamp)),
                    UserFullName = p.User.FullName,
                    UserProfileImageUrl = p.User.ProfilePath,
                    UserId = p.UserId,
                })
                .ToListAsync();

            return posts;
        }


        public async Task LikePost(int post_id, int user_id)
        {
            var existingLike = await _context.likes.FirstOrDefaultAsync(l => l.PostId == post_id && l.UserId == user_id);
            if (existingLike != null)
            {
                throw new InvalidOperationException("User has already liked this post");
            }

            var post = await _context.posts.Include(p => p.User).FirstOrDefaultAsync(p => p.Id == post_id);
            if (post == null)
            {
                throw new InvalidOperationException("Post not found");
            }

            var user = await _context.users.FirstOrDefaultAsync(u => u.Id == user_id);
            if (user == null)
            {
                throw new InvalidOperationException("User not found");
            }

            post.LikesCount += 1;

            var like = new Like
            {
                PostId = post_id,
                UserId = user_id,
            };

            _context.likes.Add(like);


            if (user.LastLikePointsAwarded != DateTime.Today)
            {
                var likesCount = await _context.likes.CountAsync(l => l.UserId == user_id && l.CreatedAt >= DateTime.Today);

                if (likesCount >= 5)
                {
                    user.LastLikePointsAwarded = DateTime.Today;
                    user.points += 20;

                    if (user.points >= 1000)
                    {
                        user.Level += 1;
                        user.points = 0;
                    }

                    _context.users.Update(user);
                }
            }

            await _context.SaveChangesAsync();

            Console.WriteLine($"Post liked by user {user_id}. Sending notification to user {post.UserId}.");

            var connectionIds = ChatHub.ConnectedUsers.Where(kvp => kvp.Value == post.UserId.ToString()).Select(kvp => kvp.Key).ToList();
            if (connectionIds.Count > 0)
            {
                foreach (var connectionId in connectionIds)
                {
                    Console.WriteLine($"Sending notification to connection ID {connectionId}");
                    await _hubContext.Clients.Client(connectionId).SendAsync("ReceiveNotification", $"Your post has been liked by user {user_id}");
                }
                Console.WriteLine($"Notification sent to user {post.UserId}.");
            }
            else
            {
                Console.WriteLine($"User {post.UserId} is not connected.");
            }
        }

        public async Task DislikePost(int post_id, int user_id)
        {
            var like = await _context.likes.FirstOrDefaultAsync(l => l.PostId == post_id && l.UserId == user_id);
            if (like == null)
            {
                throw new InvalidOperationException("Like not found");
            }

            var post = await _context.posts.FindAsync(post_id);
            if (post == null)
            {
                throw new InvalidOperationException("Post not found");
            }

            post.LikesCount -= 1;

            _context.likes.Remove(like);
            await _context.SaveChangesAsync();
        }

        public async Task LikeComment(int comment_id,int user_id)
        {
            var existingLike = await _context.likes.FirstOrDefaultAsync(l => l.CommentId== comment_id && l.UserId == user_id);
            if (existingLike != null)
            {
                throw new InvalidOperationException("User has already liked this comment");
            }
            var comment = await _context.comments.FindAsync(comment_id);
            if (comment == null)
            {
                throw new InvalidOperationException("Comment not found");
            }
            comment.LikesCount += 1;

            var like = new Like
            {
                CommentId=comment_id,
                UserId = user_id,

            };

            _context.likes.Add(like);
            await _context.SaveChangesAsync();

        }

        public async Task DislikeComment(int comment_id, int user_id)
        {
            var like = await _context.likes.FirstOrDefaultAsync(l => l.CommentId == comment_id && l.UserId == user_id);
            if (like == null)
            {
                throw new InvalidOperationException("Like not found");
            }

            var comment = await _context.comments.FindAsync(comment_id);
            if (comment == null)
            {
                throw new InvalidOperationException("Comment not found");
            }

            comment.LikesCount -= 1;

            _context.likes.Remove(like);
            await _context.SaveChangesAsync();
        }

        public async Task CommentOnPost(int post_id, int user_id, CommentDto dto)
        {
            var post = await _context.posts.FindAsync(post_id);
            if (post == null)
            {
                throw new InvalidOperationException("Post not found");
            }
            post.CommentsCount += 1;

            var comment = new Comment
            {
                Description = dto.Description,
                ImageUrl = dto.ImageUrl,
                LikesCount = 0,
                PostId = post_id,
                UserId = user_id,
                time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
            };

            _context.comments.Add(comment);

            var user = await _context.users.FirstOrDefaultAsync(u => u.Id == user_id);
            if (user == null)
            {
                throw new InvalidOperationException("User not found");
            }

            if (user.LastCmntPointsAwarded != DateTime.Today)
            {
                var today = DateTime.Today;
                var userComments = await _context.comments
                    .Where(l => l.UserId == user_id)
                    .ToListAsync();

                var CmntsCount = userComments
                    .Count(l => DateTime.TryParse(l.time, out DateTime parsedDate) && parsedDate >= today);

                if (CmntsCount >= 5)
                {
                    user.LastCmntPointsAwarded = DateTime.Today;
                    user.points += 20;

                    if (user.points >= 1000)
                    {
                        user.Level += 1;
                        user.points = 0;
                    }

                    _context.users.Update(user);
                }
            }

            await _context.SaveChangesAsync();
        }


        public async Task DeleteComment(int comment_id)
        {
            var comment = await _context.comments.FindAsync(comment_id);
            if (comment == null)
            {
                throw new InvalidOperationException("Comment not found");
            }

            var post = await _context.posts.FindAsync(comment.PostId);
            if (post == null)
            {
                throw new InvalidOperationException("Post associated with the comment not found");
            }

            post.CommentsCount -= 1;

            _context.comments.Remove(comment);
            await _context.SaveChangesAsync();
        }

        public async Task<int> PostLikesNb(int post_id)
        {
            var post = await _context.posts.FindAsync(post_id);
            if (post == null)
            {
                throw new InvalidOperationException("Post not found");
            }
            var nb = post.LikesCount;
            return nb;
        }

        public async Task<int> CommentLikesNb(int comment_id)
        {
            var comment = await _context.posts.FindAsync(comment_id);
            if (comment == null)
            {
                throw new InvalidOperationException("Comment not found");

            }
            var nb = comment.LikesCount;

            return nb;
        }

        public async Task<int> PostCommentsNb(int post_id)
        {
            var post = await _context.posts.FindAsync(post_id);
            if (post == null)
            {
                throw new InvalidOperationException("Post not found");
            }
            var nb = post.CommentsCount;
            return nb;
        }

        public async Task<IEnumerable<GetCommentDto>> GetAllCommentsWithDetails(int postId)
        {
            var commentsWithDetails = await _context.comments
                .Where(comment => comment.PostId == postId)  
                .Include(comment => comment.User)
                .Select(comment => new GetCommentDto
                {
                    id=comment.Id,
                    Description = comment.Description,
                    ImageUrl = comment.ImageUrl,
                    UserName = comment.User.FullName,
                    UserProfilePath = comment.User.ProfilePath,
                    LikesCount = comment.LikesCount,
                    time = CalculateTimeAgo(DateTime.Parse(comment.time))
                })
                .ToListAsync();

            return commentsWithDetails;
        }


        public async Task<bool> HasUserLikedPost(int postId, int userId)
        {
            var existingLike = await _context.likes
                .FirstOrDefaultAsync(l => l.PostId == postId && l.UserId == userId);

            return existingLike != null;
        }

        public async Task<bool> HasUserLikedComment(int commentId, int userId)
        {
            var existingLike = await _context.likes
                .FirstOrDefaultAsync(l => l.CommentId == commentId && l.UserId == userId);

            return existingLike != null;
        }

        public async Task<List<GetPostDto>> GetPostsOfAllSubcommunities(int preCommunityId)
        {
        
            var subCommunities = await _context.sub_communities
                .Where(sc => sc.CommunityID == preCommunityId)
                .Select(sc => sc.ID)
                .ToListAsync();

          
            var posts = await _context.posts
                .Where(p => subCommunities.Contains((int)p.SubCommunityId))
                .Select(p => new GetPostDto
                {
                    Id = p.Id,
                    Description = p.Description,
                    ImageUrl = p.ImageUrl,
                    LikesCount = p.LikesCount,
                    CommentsCount = p.CommentsCount,
                    ShareCount = p.ShareCount,
                    Timestamp = CalculateTimeAgo(DateTime.Parse(p.Timestamp)),
                    UserFullName = p.User.FullName,
                    UserProfileImageUrl = p.User.ProfilePath,
                    UserId = p.UserId
                })
                .ToListAsync();

            return posts;
        }

        public async Task<List<GetPostDto>> GetUserPosts(int userId)
        {
            var user = await _context.users.FindAsync(userId);
            if (user == null)
            {
                throw new InvalidOperationException("User not found");
            }

            var posts = await _context.posts
                .Where(p => p.UserId == userId)
                .Select(p => new GetPostDto
                {
                    Id = p.Id,
                    Description = p.Description,
                    ImageUrl = p.ImageUrl,
                    LikesCount = p.LikesCount,
                    CommentsCount = p.CommentsCount,
                    ShareCount = p.ShareCount,
                    Timestamp = CalculateTimeAgo(DateTime.Parse(p.Timestamp)),
                    UserFullName = p.User.FullName,
                    UserProfileImageUrl = p.User.ProfilePath
                })
                .ToListAsync();

            return posts;

        }
    }






}

