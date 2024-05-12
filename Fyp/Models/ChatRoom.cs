using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Fyp.Models
{
    public class ChatRoom
    {
        public int Id { get; set; }

        [Required]
        public string RoomName { get; set; }

        public string Description { get; set; }
        [JsonIgnore]
        public ICollection<Message> Messages { get; set; }
        [JsonIgnore]
        public ICollection<UserChatRoom> UserChatRooms { get; set; }
    }
}
