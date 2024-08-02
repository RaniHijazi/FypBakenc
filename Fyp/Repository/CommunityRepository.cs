using Fyp.Dto;
using Fyp.Interfaces;
using Fyp.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Fyp.Repository
{
    public class CommunityRepository: ICommunityRepository
    {
        private readonly DataContext _context;
        private readonly BlobStorageService _blobStorageService;
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly IFcmService _fcmService;

        public CommunityRepository(DataContext context, BlobStorageService blobStorageService, IHubContext<ChatHub> hubContext, IFcmService fcmService)
        {
            _context = context;
            _blobStorageService = blobStorageService;
            _hubContext = hubContext;
            _fcmService = fcmService;

        }

        public async Task CreateCommunity(CommunityDto dto)
        {
            if (dto.Name == null)
            {
                throw new InvalidOperationException("community name is null");
                
            }

            if (dto.Name == null)
            {
                throw new InvalidOperationException("community description is null");

            }
            var precommunity = new Community
            {
                Name = dto.Name,
                Description = dto.Description

            };

            _context.communities.Add(precommunity);
            await _context.SaveChangesAsync();
        }

        public async Task CreateSubCommunity(int preId,string name,string description,IFormFile? image)
        {
            var precommunity = await _context.communities.FirstOrDefaultAsync(pre => pre.Id == preId);
            if (precommunity == null)
            {
                throw new InvalidOperationException("Didn't found the main community");
            }

            string imageUrl = null;
            if (image != null)
            {

                imageUrl = await _blobStorageService.UploadImageAsync(image);
            }

            var presub = new SubCommunity
            {
                Name = name,
                ImageUrl = imageUrl,
                CommunityID = preId,
                NBMembers = 0,
                Description=description,
                Status="Inactive",
            
            };



            _context.sub_communities.Add(presub);
            await _context.SaveChangesAsync();
        }


        public async Task CreateAdminSubCommunity(string communityname, string name, string description, IFormFile? image)
        {
            var precommunity = await _context.communities.FirstOrDefaultAsync(pre => pre.Name == communityname);
            if (precommunity == null)
            {
                throw new InvalidOperationException("Didn't found the main community");
            }

            string imageUrl = null;
            if (image != null)
            {

                imageUrl = await _blobStorageService.UploadImageAsync(image);
            }
            var communityId = await _context.communities
            .Where(c => c.Name == communityname)
            .Select(c => c.Id)
               .FirstOrDefaultAsync();

            var presub = new SubCommunity
            {
                Name = name,
                ImageUrl = imageUrl,
                CommunityID = communityId,
                NBMembers = 0,
                Description = description,
                Status = "Inactive",

            };

            _context.sub_communities.Add(presub);
            await _context.SaveChangesAsync();
        }

        public async Task AddUsersToSubCommunity(List<string> userNames, int subCommunityId)
        {

            var users = await _context.users.Where(u => userNames.Contains(u.FullName)).ToListAsync();

            if (users.Count != userNames.Count)
            {
                var missingUsers = userNames.Except(users.Select(u => u.FullName)).ToList();
                throw new InvalidOperationException($"The following users were not found: {string.Join(", ", missingUsers)}");
            }


            var subCommunity = await _context.sub_communities.FindAsync(subCommunityId);
            if (subCommunity == null)
            {
                throw new InvalidOperationException("SubCommunity not found!");
            }

            subCommunity.NBMembers +=users.Count;

            var userSubCommunities = users.Select(user => new UserSubCommunity
            {
                UserId = user.Id,
                User = user,
                SubCommunityId = subCommunityId,
                SubCommunity = subCommunity
            }).ToList();

            _context.user_sub_communities.AddRange(userSubCommunities);
            await _context.SaveChangesAsync();
        }


        public async Task RemoveUsersFromSubCommunity(List<string> userNames, int subCommunityId)
        {
            var users = await _context.users.Where(u => userNames.Contains(u.FullName)).ToListAsync();

            if (users.Count != userNames.Count)
            {
                var missingUsers = userNames.Except(users.Select(u => u.FullName)).ToList();
                throw new InvalidOperationException($"The following users were not found: {string.Join(", ", missingUsers)}");
            }

            var subCommunity = await _context.sub_communities.FindAsync(subCommunityId);
            if (subCommunity == null)
            {
                throw new InvalidOperationException("SubCommunity not found!");
            }
            subCommunity.NBMembers = subCommunity.NBMembers - users.Count;
            var userSubCommunities = await _context.user_sub_communities
                .Where(usc => usc.SubCommunityId == subCommunityId && userNames.Contains(usc.User.FullName))
                .ToListAsync();

            if (userSubCommunities.Count != userNames.Count)
            {
                var missingUserSubCommunities = userNames.Except(userSubCommunities.Select(usc => usc.User.FullName)).ToList();
                throw new InvalidOperationException($"The following users are not part of the subcommunity: {string.Join(", ", missingUserSubCommunities)}");
            }
            _context.user_sub_communities.RemoveRange(userSubCommunities);
            await _context.SaveChangesAsync();
        }

        public async Task AddUserToSubCommunity(int userId, int subCommunityId)
        {
            var user = await _context.users.FindAsync(userId);

            if (user == null)
            {
                throw new InvalidOperationException("User not found!");
            }

            var subCommunity = await _context.sub_communities.FindAsync(subCommunityId);
            if (subCommunity == null)
            {
                throw new InvalidOperationException("SubCommunity not found!");
            }

            subCommunity.NBMembers += 1;

            var userSubCommunity = new UserSubCommunity
            {
                UserId = user.Id,
                User = user,
                SubCommunityId = subCommunityId,
                SubCommunity = subCommunity
            };

            _context.user_sub_communities.Add(userSubCommunity);
            await _context.SaveChangesAsync();
        }


        public async Task RemoveUserFromSubCommunity(int userId, int subCommunityId)
        {
            var userSubCommunity = await _context.user_sub_communities
                .FirstOrDefaultAsync(usc => usc.UserId == userId && usc.SubCommunityId == subCommunityId);

            if (userSubCommunity == null)
            {
                throw new InvalidOperationException("User is not a member of this sub-community!");
            }

            _context.user_sub_communities.Remove(userSubCommunity);

            var subCommunity = await _context.sub_communities.FindAsync(subCommunityId);
            if (subCommunity != null)
            {
                subCommunity.NBMembers -= 1;
            }

            await _context.SaveChangesAsync();
        }






        public async Task DeleteSubCommunity(int subCommunityId)
        {
            var subCommunity = await _context.sub_communities
                .Include(sc => sc.UserSubCommunities)
                .Include(sc => sc.Posts)
                .FirstOrDefaultAsync(sc => sc.ID == subCommunityId);

            if (subCommunity == null)
            {
                throw new InvalidOperationException("Subcommunity not found");
            }

            _context.user_sub_communities.RemoveRange(subCommunity.UserSubCommunities);
            _context.posts.RemoveRange(subCommunity.Posts);
            _context.sub_communities.Remove(subCommunity);

            await _context.SaveChangesAsync();
        }

        public async Task<List<SubCommunityDto>> GetSubCommunities(int preCommunityId)
        {
            var sub_communities = await _context.sub_communities
                                                .Where(sub => sub.CommunityID == preCommunityId && sub.Status == "Active")
                                                .Select(sub => new SubCommunityDto
                                                {
                                                    Id = sub.ID,
                                                    Name = sub.Name,
                                                    ImageUrl = sub.ImageUrl,
                                                    Description = sub.Description,
                                                    NBMembers = sub.NBMembers 
                                                })
                                                .ToListAsync();

            return sub_communities;
        }


        public async Task<List<SubAdminDto>> GetAdminSubCommunities()
        {
            var sub_communities = await _context.sub_communities
                                                .Select(sub => new SubAdminDto
                                                {
                                                    Id = sub.ID,
                                                    Name = sub.Name,
                                                    MainCommunityName= sub.MainCommunity.Name,
                                                    Status = sub.Status,
                                                    NBMembers = sub.NBMembers
                                                })
                                                .ToListAsync();

            return sub_communities;
        }

        public async Task<string> ToggleCommunityStatus(int communityId)
        {
            var community = await _context.sub_communities.FindAsync(communityId);
            if (community == null)
            {
                return null;
            }

            community.Status = community.Status == "Active" ? "Inactive" : "Active";
            await _context.SaveChangesAsync();

            
            return community.Status;
        }

        public async Task<bool> IsUserMemberOfSubCommunity(int userId, int subCommunityId)
        {
            var userSubCommunity = await _context.user_sub_communities
                .FirstOrDefaultAsync(usc => usc.UserId == userId && usc.SubCommunityId == subCommunityId);

            return userSubCommunity != null;
        }

    }
}
