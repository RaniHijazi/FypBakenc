using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Fyp.Models
{
    public class SubCommunity
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int NBMembers { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string? ImageUrl { get; set; }
        [ForeignKey("MainCommunity")]
        public int CommunityID { get; set; }

        [JsonIgnore]
        public Community MainCommunity { get; set; }

        [JsonIgnore]
        public ICollection<UserSubCommunity> UserSubCommunities { get; set; }
        [JsonIgnore]
        public ICollection<Post> Posts { get; set; }
    }
}
