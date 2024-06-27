namespace Fyp.Dto
{
    public class GetPostDto
    {
        public int Id { get; set; } 
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public int LikesCount { get; set; }
        public int CommentsCount { get; set; }
        public int ShareCount { get; set; }
        public string Timestamp { get; set; }
        public string UserFullName { get; set; }
        public string UserProfileImageUrl { get; set; }
        public int UserId { get; set; }
    }
}
