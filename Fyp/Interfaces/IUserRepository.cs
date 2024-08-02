using System.Collections.Generic;
using System.Threading.Tasks;
using Fyp.Dto;
using Fyp.Models;

namespace Fyp.Interfaces
{
    public interface IUserRepository
    {
        Task<bool> VerifyVerificationCode(string userEmail, string verificationCode);
        Task<User> SignUp(SignUpDto dto);
        Task<User> SignIn(SignInDto dto);
        Task<User> GetUserByIdAsync(int userId);
        Task<List<User>> GetAllUsers();
        Task<bool> UpdateUserProfile(int userId, UpdateUserDto dto);
        Task FollowUserAsync(int followerId, int followedId);
        Task UnfollowUserAsync(int followerId, int followedId);
        Task<UserProfileDto> GetUserProfile(int userId);
        Task<bool> IsFollowingAsync(int followerId, int followedId);
        Task<List<GetUserStoriesDto>> GetStoriesAsync(int userId);
        Task<List<GetStoryDto>> GetUserStoriesByIdAsync(int userId);
        Task<List<FollowingUserDto>> GetFollowingUsersAsync(int userId);
        Task<List<UserDto>> GetUsersAdmin();
        Task DeleteOldStoriesAsync();
        Task<string> ToggleUserStatus(int userId);
        Task<bool> UpdateUserDetails(int userId, UserUpdateDto userDetails);
        Task<List<string>> GetUserNamesInSubCommunityAsync(int subCommunityId);
        Task<List<string>> GetUserNamesNotInSubCommunityAsync(int communityId);
        Task<bool> DeleteUserByIdAndPassword(int userId, string password);
        Task<bool> ChangePassword(int userId, string oldPassword, string newPassword);
        Task<int> GetDailyPostCountAsync(int userId);
        Task<int> GetUserPointsAsync(int userId);
        Task<int> GetDailyLikesCountAsync(int userId);
        Task<int> GetDailyCommentsCountAsync(int userId);
        Task<int> GetUserLevelAsync(int userId);

    }
}
