using System.Text.Json.Serialization;

namespace Fyp.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public int LikesCount { get; set; } = 0;
        public int UserId { get; set; }
        [JsonIgnore]
        public User User { get; set; }
        public int PostId { get; set; }
        [JsonIgnore]
        public Post Post { get; set; }
        [JsonIgnore]
        public ICollection<Like> Likes { get; set; }
        public string time { get; set; }
    }
}
