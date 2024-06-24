using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Fyp.Models
{
    public class Message
    {
        public int MessageId { get; set; }

        public int SenderId { get; set; }

        public int? RecipientId { get; set; }

        public int? RoomId { get; set; }

        [Required]
        public string Content { get; set; }

        public DateTime Timestamp { get; set; }

        [JsonIgnore]
        public User Sender { get; set; }
        [JsonIgnore]
        public User? Recipient { get; set; }

        public ChatRoom Room { get; set; }
        public int? StoryId { get; set; }
        [JsonIgnore]
        public Story Story { get; set; }


    }
}