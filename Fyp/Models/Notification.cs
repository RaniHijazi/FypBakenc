using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Fyp.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Content { get; set; }
        public DateTime Time { get; set; }

        [ForeignKey("UserId")]
        [JsonIgnore]
        public User User { get; set; }
    }
}
