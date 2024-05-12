using System.Text.Json.Serialization;

namespace Fyp.Models
{
    public class UserSubCommunity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        [JsonIgnore]
        public User User { get; set; }
        public int SubCommunityId { get; set; }
        [JsonIgnore]
        public PreSubCommunity SubCommunity { get; set; }
    }
}
