using Fyp.Dto;
using Fyp.Models;

namespace Fyp.Repository
{
    public interface IPreCommunityRepository
    {
        Task CreatePreCommunity(PreCommunityDto dto);
        Task CreatePreSubCommunity(int preId, string name);
        Task AddUserToPreSubCommunity(int userId, int presubCommunityId);
        Task DeleteSubCommunity(int subCommunityId);
        Task<List<PreSubCommunity>> GetPreSubCommunities(int precommunityId);
    }
}
