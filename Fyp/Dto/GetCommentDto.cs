namespace Fyp.Dto
{
    public class GetCommentDto
    {
        public int id { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public string UserName { get; set; }
        public string? UserProfilePath { get; set; }
        public int LikesCount { get; set; }
        public string time { get; set; }
        public int Level { get; set; }
    }
}
