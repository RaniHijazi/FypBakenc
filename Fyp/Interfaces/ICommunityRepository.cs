using Fyp.Dto;
using Fyp.Models;

namespace Fyp.Repository
{
    public interface ICommunityRepository
    {
        Task CreateCommunity(CommunityDto dto);
        Task CreateSubCommunity(int preId, string name);
        Task AddUserToSubCommunity(int userId, int presubCommunityId);
        Task DeleteSubCommunity(int subCommunityId);
        Task<List<SubCommunity>> GetSubCommunities(int precommunityId);
    }
}
