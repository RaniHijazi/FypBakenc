using Fyp.Dto;
using Fyp.Models;

namespace Fyp.Repository
{
    public interface ICommunityRepository
    {
        Task CreateCommunity(CommunityDto dto);
        Task CreateSubCommunity(int preId, string name, string description, IFormFile? image);
        Task AddUserToSubCommunity(int userId, int presubCommunityId);
        Task DeleteSubCommunity(int subCommunityId);
        Task<List<SubCommunityDto>> GetSubCommunities(int precommunityId);
    }
}
