using Fyp.Dto;
using Fyp.Interfaces;
using Fyp.Models;
using Microsoft.EntityFrameworkCore;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Fyp.Repository
{
    public class UserRepository : IUserRepository
    {

        private readonly DataContext _context;
        private readonly BlobStorageService _blobStorageService;

        public UserRepository(DataContext context, BlobStorageService blobStorageService)
        {
            _context = context;
            _blobStorageService = blobStorageService;
        }

        public async Task<bool> VerifyVerificationCode(string userEmail, string verificationCode)
        {
            var user = await _context.users.FirstOrDefaultAsync(u => u.Email == userEmail);

            if (user == null || user.VerificationCode != verificationCode)
            {
                return false;
            }
            return true;
        }

        public async Task<User> SignUp(SignUpDto dto)
        {
            bool isUAEduLbEmail = dto.Email.EndsWith("@ua.edu.lb");
            string role = isUAEduLbEmail ? "student" : "default_role";

            if (await _context.users.AnyAsync(u => u.FullName == dto.FullName))
            {
                throw new InvalidOperationException("Username already exists");
            }

            if (await _context.users.AnyAsync(u => u.Email == dto.Email))
            {
                throw new InvalidOperationException("Email already used");
            }

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            int CommunityId = isUAEduLbEmail ? 2 : 1;

            var newUser = new User
            {
                FullName = dto.FullName,
                Email = dto.Email,
                Password = hashedPassword,
                Gender = dto.Gender,
                Age = dto.Age,
                JoinDate = DateTime.Now,
                Role = role,
                CommunityId = CommunityId,
                ProfilePath = "https://ranistoragefyp.blob.core.windows.net/blobfyp/depositphotos_134255634-stock-illustration-avatar-icon-male-profile-gray.jpg"
            };

            _context.users.Add(newUser);
            await _context.SaveChangesAsync();


            var documents = new List<Document>
    {
        new Document
        {

            Name = "Document 1",
            Status = "Pending",
            Description = "Description of Document 1",
            UploadDate = DateTime.Now.ToString("yyyy-MM-dd"),
            UserId = newUser.Id
        },
        new Document
        {

            Name = "Document 2",
            Status = "Pending",
            Description = "Description of Document 2",
            UploadDate = DateTime.Now.ToString("yyyy-MM-dd"),
            UserId = newUser.Id
        },
        new Document
        {

            Name = "Document 3",
            Status = "Pending",
            Description = "Description of Document 3",
            UploadDate = DateTime.Now.ToString("yyyy-MM-dd"),
            UserId = newUser.Id
        },
        new Document
        {

            Name = "Document 4",
            Status = "Pending",
            Description = "Description of Document 4",
            UploadDate = DateTime.Now.ToString("yyyy-MM-dd"),
            UserId = newUser.Id
        }
    };

            _context.documents.AddRange(documents);
            await _context.SaveChangesAsync();

            return newUser;
        }


        public async Task<User> GetUserByIdAsync(int userId)
        {
            return await _context.users.FindAsync(userId);
        }

        public async Task<List<string>> GetUserNamesInSubCommunityAsync(int subCommunityId)
        {
            return await (from u in _context.users
                          join usc in _context.user_sub_communities on u.Id equals usc.UserId
                          where usc.SubCommunityId == subCommunityId
                          orderby u.FullName
                          select u.FullName).ToListAsync();
        }


        public async Task<List<string>> GetUserNamesNotInSubCommunityAsync(int communityId)
        {
            return await (from u in _context.users
                          join usc in _context.user_sub_communities on u.Id equals usc.UserId into userSubCommunities
                          from subCommunity in userSubCommunities.DefaultIfEmpty()
                          where subCommunity == null || subCommunity.SubCommunityId != communityId
                          orderby u.FullName
                          select u.FullName).ToListAsync();
        }





        public async Task<User> SignIn(SignInDto dto)
        {
            var user = await _context.users.FirstOrDefaultAsync(u => u.FullName == dto.FullName);

            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
            {
                throw new InvalidOperationException("Invalid username or password");
            }

            if (user.MemberStatus != "Active")
            {
                throw new InvalidOperationException("Error sign in, account deactivated");
            }

            return user;
        }


        public async Task<List<User>> GetAllUsers()
        {
            return await _context.users.ToListAsync();
        }

        public async Task<bool> UpdateUserProfile(int userId, UpdateUserDto dto)
        {
            var user = await _context.users.FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return false;
            }

            if (!string.IsNullOrEmpty(dto.NFullName) && dto.NFullName != user.FullName)
            {
                if (await _context.users.AnyAsync(u => u.FullName == dto.NFullName))
                {
                    throw new InvalidOperationException("Username already exists");
                }

                user.FullName = dto.NFullName;
            }

            if (!string.IsNullOrEmpty(dto.NPassword))
            {
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.NPassword);
                user.Password = hashedPassword;
            }

            _context.users.Update(user);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task FollowUserAsync(int followerId, int followedId)
        {
            if (followerId == followedId)
                throw new InvalidOperationException("A user cannot follow themselves.");

            var follow = new Follow
            {
                FollowerId = followerId,
                FollowedId = followedId,
                FollowedDate = DateTime.UtcNow
            };

            _context.Follows.Add(follow);
            var userToFollow = await _context.users.FindAsync(followedId);
            if (userToFollow == null)
            {
                throw new InvalidOperationException("User followed not found");
            }

            userToFollow.TotalFollowers++;

            var followerUser = await _context.users.FindAsync(followerId);

            followerUser.TotalFollowing++;
            await _context.SaveChangesAsync();
        }

        public async Task UnfollowUserAsync(int followerId, int followedId)
        {
            var follow = await _context.Follows
                .FirstOrDefaultAsync(f => f.FollowerId == followerId && f.FollowedId == followedId);

            if (follow != null)
            {
                _context.Follows.Remove(follow);
                var userToUnfollow = await _context.users.FindAsync(followedId);
                userToUnfollow.TotalFollowers--;

                var followerUser = await _context.users.FindAsync(followerId);
                followerUser.TotalFollowing--;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<UserProfileDto> GetUserProfile(int userId)
        {
            if (userId == null)
            {
                throw new InvalidOperationException("userId null");
            }

            var user = await _context.users.FindAsync(userId);

            return new UserProfileDto
            {
                Id = user.Id,
                Bio = user.Bio,
                FullName = user.FullName,
                ProfilePath = user.ProfilePath,
                Role = user.Role,
                TotalFollowers = user.TotalFollowers,
                TotalFollowing = user.TotalFollowing,
                JoinDate = user.JoinDate

            };
        }


        public async Task<bool> IsFollowingAsync(int followerId, int followedId)
        {
            return await _context.Follows
                .AnyAsync(f => f.FollowerId == followerId && f.FollowedId == followedId);
        }



        public async Task<List<GetUserStoriesDto>> GetStoriesAsync(int userId)
        {
            var followedUserIds = await _context.Follows
                .Where(f => f.FollowerId == userId)
                .Select(f => f.FollowedId)
                .ToListAsync();

            var stories = await _context.stories
                .Where(s => followedUserIds.Contains(s.UserId))
                .Include(s => s.User)
                .OrderByDescending(s => s.CreatedAt)
                .ToListAsync();

            var groupedStories = stories
                .GroupBy(s => new { s.UserId, s.User.FullName, s.User.ProfilePath })
                .Select(g => new GetUserStoriesDto
                {
                    UserId = g.Key.UserId,
                    UserFullName = g.Key.FullName,
                    UserProfileImageUrl = g.Key.ProfilePath,
                    Stories = g.Select(s => new GetStoryDto
                    {
                        Id = s.Id,
                        StoryPath = s.StoryPath,
                        CreatedAt = s.CreatedAt
                    }).ToList()
                })
                .ToList();

            return groupedStories;
        }

        public async Task<List<GetStoryDto>> GetUserStoriesByIdAsync(int userId)
        {
            var userStories = await _context.stories
                .Where(s => s.UserId == userId)
                .OrderByDescending(s => s.CreatedAt)
                .ToListAsync();

            var storyDtos = userStories.Select(s => new GetStoryDto
            {
                Id = s.Id,
                StoryPath = s.StoryPath,
                CreatedAt = s.CreatedAt
            }).ToList();

            return storyDtos;
        }



        public async Task<List<FollowingUserDto>> GetFollowingUsersAsync(int userId)
        {
            var followingUsers = await _context.Follows
                .Where(f => f.FollowerId == userId)
                .Include(f => f.Followed)
                .Select(f => new FollowingUserDto
                {
                    Id = f.Followed.Id,
                    FullName = f.Followed.FullName,
                    ProfilePath = f.Followed.ProfilePath
                })
                .ToListAsync();

            return followingUsers;
        }

        public async Task DeleteOldStoriesAsync()
        {
            var now = DateTime.UtcNow;
            var oldStories = await _context.stories
                .Where(s => s.CreatedAt < now.AddHours(-24))
                .ToListAsync();

            if (oldStories.Any())
            {
                _context.stories.RemoveRange(oldStories);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<UserDto>> GetUsersAdmin()
        {
            var users = await _context.users.Select(u => new UserDto
            {
                Id = u.Id,
                Email = u.Email,
                FullName = u.FullName,
                Role = u.Role,
                MemberStatus = u.MemberStatus,

            })
                .ToListAsync();
            return users;
        }

        public async Task<string> ToggleUserStatus(int userId)
        {
            var user = await _context.users.FindAsync(userId);
            if (user == null)
            {
                return null;
            }

            user.MemberStatus = user.MemberStatus == "Active" ? "Inactive" : "Active";
            await _context.SaveChangesAsync();
            return user.MemberStatus;
        }

        public async Task<bool> UpdateUserDetails(int userId, UserUpdateDto userDetails)
        {
            var user = await _context.users.FindAsync(userId);
            if (user == null)
            {
                return false;
            }

            user.FullName = userDetails.FullName ?? user.FullName;
            user.Email = userDetails.Email ?? user.Email;
            user.Role = userDetails.Role ?? user.Role;

            _context.users.Update(user);
            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<bool> DeleteUserByIdAndPassword(int userId, string password)
        {
            var user = await _context.users
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                return false;
            }


            var userSubCommunities = _context.user_sub_communities.Where(usc => usc.UserId == userId);
            var userPosts = _context.posts.Where(p => p.UserId == userId);
            var userComments = _context.comments.Where(c => c.UserId == userId);
            var userLikes = _context.likes.Where(l => l.UserId == userId);
            var userMessages = _context.messages.Where(m => m.SenderId == userId || m.RecipientId == userId);
            var userChatRooms = _context.user_chat_rooms.Where(ucr => ucr.UserId == userId);
            var userStories = _context.stories.Where(s => s.UserId == userId);
            var userDocuments = _context.documents.Where(d => d.UserId == userId);
            var userFollows = _context.Follows.Where(f => f.FollowerId == userId || f.FollowedId == userId);

            _context.user_sub_communities.RemoveRange(userSubCommunities);
            _context.posts.RemoveRange(userPosts);
            _context.comments.RemoveRange(userComments);
            _context.likes.RemoveRange(userLikes);
            _context.messages.RemoveRange(userMessages);
            _context.user_chat_rooms.RemoveRange(userChatRooms);
            _context.stories.RemoveRange(userStories);
            _context.documents.RemoveRange(userDocuments);
            _context.Follows.RemoveRange(userFollows);

            _context.users.Remove(user);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<int> GetUserPointsAsync(int userId)
        {
            var user = await _context.users.FindAsync(userId);
            return user != null ? user.points : 0;
        }

        public async Task<int> GetDailyLikesCountAsync(int userId)
        {
            var user = await _context.users.FindAsync(userId);

            if (user == null)
            {
                throw new InvalidOperationException("User not found");
            }

            if (user.LastLikePointsAwarded != DateTime.Today)
            {
                var today = DateTime.Today;
                var likesCount = await _context.likes
                    .CountAsync(l => l.UserId == userId && l.CreatedAt >= today);

                return likesCount;
            }
            else
            {
                return 5;
            }
        }

        public async Task<int> GetDailyPostCountAsync(int userId)
        {
            var user = await _context.users.FindAsync(userId);

            if (user == null)
            {
                throw new InvalidOperationException("User not found");
            }

            if (user.LastPostPointsAwarded != DateTime.Today)
            {
                var today = DateTime.Today;
                var userPosts = await _context.posts
                    .Where(l => l.UserId == userId)
                    .ToListAsync();

                var postsCount = userPosts
                    .Count(l => DateTime.TryParse(l.Timestamp, out DateTime parsedDate) && parsedDate >= today);

                return postsCount;
            }
            else
            {
                return 1;
            }
        }

        public async Task<int> GetDailyCommentsCountAsync(int userId)
        {
            var user = await _context.users.FindAsync(userId);

            if (user == null)
            {
                throw new InvalidOperationException("User not found");
            }

            if (user.LastCmntPointsAwarded != DateTime.Today)
            {
                var today = DateTime.Today;
                var userComments = await _context.comments
                    .Where(l => l.UserId == userId)
                    .ToListAsync();

                var commentsCount = userComments
                    .Count(l => DateTime.TryParse(l.time, out DateTime parsedDate) && parsedDate >= today);

                return commentsCount;
            }
            else
            {
                return 5;
            }
        }


        public async Task<int> GetUserLevelAsync(int userId)
        {
            var user = await _context.users.FindAsync(userId);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            return user.Level;
        }



        public async Task<bool> ChangePassword(int userId, string oldPassword, string newPassword)
        {
            var user = await _context.users.FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null || !BCrypt.Net.BCrypt.Verify(oldPassword, user.Password))
            {
                return false; 
            }

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(newPassword);
            user.Password = hashedPassword;

            _context.users.Update(user);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}