using System.Text.Json.Serialization;

namespace Fyp.Models
{
    public class UserChatRoom
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        [JsonIgnore]
        public User User { get; set; }

        public int RoomId { get; set; }
        [JsonIgnore]
        public ChatRoom Room { get; set; }
    }
}
