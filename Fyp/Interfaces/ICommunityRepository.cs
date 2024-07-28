using Fyp.Dto;
using Fyp.Models;

namespace Fyp.Repository
{
    public interface ICommunityRepository
    {
        Task CreateCommunity(CommunityDto dto);
        Task CreateSubCommunity(int preId, string name, string description, IFormFile? image);
        Task CreateAdminSubCommunity(string Maincommunityname, string name, string description, IFormFile? image);
        Task AddUsersToSubCommunity(List<string> userNames, int subCommunityId);
        Task RemoveUsersFromSubCommunity(List<string> userNames, int subCommunityId);
        Task DeleteSubCommunity(int subCommunityId);
        Task<List<SubCommunityDto>> GetSubCommunities(int precommunityId);
        Task<List<SubAdminDto>> GetAdminSubCommunities();
        Task<string> ToggleCommunityStatus(int userId);
        Task AddUserToSubCommunity(int userId, int subCommunityId);
        Task RemoveUserFromSubCommunity(int userId, int subCommunityId);
        Task<bool> IsUserMemberOfSubCommunity(int userId, int subCommunityId);
    }
}
