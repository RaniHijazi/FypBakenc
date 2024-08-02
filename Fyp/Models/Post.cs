using System.Text.Json.Serialization;

namespace Fyp.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public int LikesCount { get; set; } = 0;
        public int CommentsCount { get; set; } = 0;
        public int ShareCount { get; set; } = 0;
        public string Timestamp { get; set; }
      

        public int UserId { get; set; }
        [JsonIgnore]
        public User User { get; set; }
        public int? CommunityId { get; set; }
        [JsonIgnore]
        public Community? Community { get; set; }
        public int? SubCommunityId { get; set; }
        [JsonIgnore]
        public SubCommunity? SubCommunity { get; set; }
        [JsonIgnore]
        public ICollection<Comment> Comments { get; set; }
        [JsonIgnore]
        public ICollection<Like> Likes { get; set; }

    }
}
