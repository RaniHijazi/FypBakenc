
using Fyp.Dto;

namespace Fyp.Repository
{
    public interface IPostRepository
    {
        Task CreatePrePost(int user_id, int precommunity_id, PostDto dto);
        Task CreatePreSubPosts(int user_id, int precommunity_id, int presubcommunity_id, PostDto dto);
        Task DeletePost(int post_id);
        Task<List<GetPostDto>> GetPrePosts(int PreCommunityId);
        Task<List<GetPostDto>> GetPreSubPosts(int PreCommunityId, int subCommunityId);
        Task LikePost(int post_id, int user_id);
        Task DislikePost(int post_id, int user_id);
        Task LikeComment(int comment_id, int user_id);
        Task DislikeComment(int comment_id, int user_id);
        Task CommentOnPost(int post_id, int user_id, CommentDto dto);
        Task DeleteComment(int comment_id);
        Task<int> PostLikesNb(int post_id);
        Task<int> CommentLikesNb(int comment_id);
        Task<int> PostCommentsNb(int post_id);
        Task<IEnumerable<GetCommentDto>> GetAllCommentsWithDetails();
    }
}
