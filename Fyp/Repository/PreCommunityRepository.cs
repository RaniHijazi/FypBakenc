using Fyp.Dto;
using Fyp.Models;
using Microsoft.EntityFrameworkCore;

namespace Fyp.Repository
{
    public class PreCommunityRepository: IPreCommunityRepository
    {
        private readonly DataContext _context;

        public PreCommunityRepository(DataContext context)
        {
            _context = context;
        }

        public async Task CreatePreCommunity(PreCommunityDto dto)
        {
            if (dto.Name == null)
            {
                throw new InvalidOperationException("community name is null");
                
            }

            if (dto.Name == null)
            {
                throw new InvalidOperationException("community description is null");

            }
            var precommunity = new PreCommunity
            {
                Name = dto.Name,
                Description = dto.Description

            };

            _context.pre_communities.Add(precommunity);
            await _context.SaveChangesAsync();
        }

        public async Task CreatePreSubCommunity(int preId,string name)
        {
            var precommunity = await _context.pre_communities.FirstOrDefaultAsync(pre => pre.Id == preId);
            if (precommunity == null)
            {
                throw new InvalidOperationException("Didn't found the main community");
            }

            var presub = new PreSubCommunity
            {
                Name = name,
                 PreCommunityID = preId,    
            };

            _context.pre_sub_communities.Add(presub);
            await _context.SaveChangesAsync();
        }

        public async Task AddUserToPreSubCommunity(int userId, int presubCommunityId)
        {
            var user = await _context.users.FindAsync(userId);
            if (user == null)
            {

                throw new InvalidOperationException("User not found!");
            }

            var subCommunity = await _context.pre_sub_communities.FindAsync(presubCommunityId);
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
            var sub=await _context.pre_sub_communities.FindAsync(subCommunityId);
            if (sub==null)
            {
                throw new InvalidOperationException("community not found");
            }
            _context.pre_sub_communities.Remove(sub);
            await _context.SaveChangesAsync();
        }

        public async Task<List<PreSubCommunity>> GetPreSubCommunities(int preCommunityId)
        {
            var sub_communities = await _context.pre_sub_communities
                                            .Where(sub => sub.PreCommunityID == preCommunityId)
                                            .ToListAsync();

            return sub_communities;
        }
    }
}
