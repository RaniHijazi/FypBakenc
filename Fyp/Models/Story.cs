using System.Text.Json.Serialization;

namespace Fyp.Models
{
    public class Story
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string StoryPath { get; set; }
        public DateTime CreatedAt { get; set; }
        [JsonIgnore]
        public User User { get; set; }
        public ICollection<Message> Messages { get; set; }
    }
}
