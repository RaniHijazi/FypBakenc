namespace Fyp.Dto
{
    public class GetPostDto
    {
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public int LikesCount { get; set; }
        public int CommentsCount { get; set; }
        public int ShareCount { get; set; }
        public string Timestamp { get; set; }
    }
}
