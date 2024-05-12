using System.Text.Json.Serialization;

namespace Fyp.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int UserId { get; set; }
        [JsonIgnore]
        public User User { get; set; }
        public int PreCommunityId { get; set; }
        [JsonIgnore]
        public PreCommunity PreCommunity { get; set; }
        public int PreSubCommunityId { get; set; }
        [JsonIgnore]
        public PreSubCommunity PreSubCommunity { get; set; }
        [JsonIgnore]
        public ICollection<Comment> Comments { get; set; }
        [JsonIgnore]
        public ICollection<Like> Likes { get; set; }

    }
}
