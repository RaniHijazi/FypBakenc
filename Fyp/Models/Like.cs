using System.Text.Json.Serialization;

namespace Fyp.Models
{
    public class Like
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }

        public int? PostId { get; set; }
        [JsonIgnore]
        public Post Post { get; set; }

        public int? CommentId { get; set; }
        [JsonIgnore]
        public Comment Comment { get; set; }
    }
}
