namespace Fyp.Dto
{
    public class GetCommentDto
    {
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public string UserName { get; set; }
        public string? UserProfilePath { get; set; }
        public int LikesCount { get; set; }
    }
}
