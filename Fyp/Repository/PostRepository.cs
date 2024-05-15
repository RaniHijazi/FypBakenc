﻿using Fyp.Dto;
using Fyp.Models;
using Microsoft.EntityFrameworkCore;

namespace Fyp.Repository
{
    public class PostRepository
    {
        private readonly DataContext _context;

        public PostRepository(DataContext context)
        {
            _context = context;
        }

        public async Task CreatePrePost(int user_id,int precommunity_id,PostDto dto)
        {
            var user=await _context.users.FindAsync(user_id);
            var precommunityId=await _context.pre_communities.FindAsync(user_id);

            if (user == null)
            {
                throw new InvalidOperationException("User not found");

            }

            if (precommunityId==null)
            {
                throw new InvalidOperationException("Community not found");

            }

            var prepost = new Post
            {
                Description=dto.Description,
                ImageUrl=dto.ImageUrl,
                LikesCount=0,
                CommentsCount=0,
                ShareCount=0,
                Timestamp= DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                UserId=user_id,
                PreCommunityId=precommunity_id
            };

             _context.posts.Add(prepost);
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
        private string CalculateTimeAgo(DateTime timestamp)
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

        public async Task<List<GetPostDto>> GetPrePosts()
        {
            var posts = await _context.posts
                .Select(p => new GetPostDto
                {
                    Description = p.Description,
                    ImageUrl = p.ImageUrl,
                    LikesCount = p.LikesCount,
                    CommentsCount = p.CommentsCount,
                    ShareCount = p.ShareCount,
                    Timestamp = CalculateTimeAgo(DateTime.Parse(p.Timestamp))
                })
                .ToListAsync();

            return posts;
        }

        public async Task LikePost(int post_id,int user_id)
        {
            var post = await _context.posts.FindAsync(post_id);
            if (post == null)
            {
                throw new InvalidOperationException("Post not found");
            }
            post.LikesCount += 1;

            var like = new Like
            {
                PostId = post_id,
                UserId=user_id,

            };

            _context.likes.Add(like);
            await _context.SaveChangesAsync();

        }
        public async Task LikeComment(int comment_id,int user_id)
        {
            var comment = await _context.posts.FindAsync(comment_id);
            if (comment == null)
            {
                throw new InvalidOperationException("Post not found");
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

        public async Task CommentOnPost(int post_id,int user_id,CommentDto dto)
        {
            var post = await _context.posts.FindAsync(post_id);
            if (post == null)
            {
                throw new InvalidOperationException("Post not found");
            }
            post.CommentsCount += 1;

            var comment = new Comment
            {
                Description=dto.Description,
                ImageUrl=dto.ImageUrl,
                LikesCount=0,
                PostId = post_id,
                UserId = user_id

            };

            _context.comments.Add(comment);
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

        public async Task<IEnumerable<GetCommentDto>> GetAllCommentsWithDetails()
        {
           
            var commentsWithDetails = await _context.comments
                .Include(comment => comment.User)
                .Select(comment => new GetCommentDto
                {
                    Description = comment.Description,
                    ImageUrl = comment.ImageUrl,
                    UserName = comment.User.FullName,
                    UserProfilePath = comment.User.ProfilePath,
                    LikesCount = comment.LikesCount
                })
                .ToListAsync();

            return commentsWithDetails;
        }
    }

}
