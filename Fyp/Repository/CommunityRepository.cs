using Fyp.Dto;
using Fyp.Models;
using Microsoft.EntityFrameworkCore;

namespace Fyp.Repository
{
    public class CommunityRepository: ICommunityRepository
    {
        private readonly DataContext _context;

        public CommunityRepository(DataContext context)
        {
            _context = context;
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

        public async Task CreateSubCommunity(int preId,string name)
        {
            var precommunity = await _context.communities.FirstOrDefaultAsync(pre => pre.Id == preId);
            if (precommunity == null)
            {
                throw new InvalidOperationException("Didn't found the main community");
            }

            var presub = new SubCommunity
            {
                Name = name,
                 CommunityID = preId,    
            };

            _context.sub_communities.Add(presub);
            await _context.SaveChangesAsync();
        }

        public async Task AddUserToSubCommunity(int userId, int presubCommunityId)
        {
            var user = await _context.users.FindAsync(userId);
            if (user == null)
            {

                throw new InvalidOperationException("User not found!");
            }

            var subCommunity = await _context.sub_communities.FindAsync(presubCommunityId);
            if (subCommunity == null)
            {
                throw new InvalidOperationException("User not found!");
            }

            var userSubCommunity = new UserSubCommunity
            {
                UserId = userId,
                User = user,
                SubCommunityId = presubCommunityId,
                SubCommunity = subCommunity
            };
            _context.user_sub_communities.Add(userSubCommunity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteSubCommunity(int subCommunityId)
        {
            var sub=await _context.sub_communities.FindAsync(subCommunityId);
            if (sub==null)
            {
                throw new InvalidOperationException("community not found");
            }
            _context.sub_communities.Remove(sub);
            await _context.SaveChangesAsync();
        }

        public async Task<List<SubCommunity>> GetSubCommunities(int preCommunityId)
        {
            var sub_communities = await _context.sub_communities
                                            .Where(sub => sub.CommunityID == preCommunityId)
                                            .ToListAsync();

            return sub_communities;
        }
    }
}
