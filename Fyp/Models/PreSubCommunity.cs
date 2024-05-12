using System.Text.Json.Serialization;

namespace Fyp.Models
{
    public class PreSubCommunity
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int PreCommunityID { get; set; }

        [JsonIgnore]
        public PreCommunity MainCommunity { get; set; }

        [JsonIgnore]
        public ICollection<UserSubCommunity> UserSubCommunities { get; set; }
        [JsonIgnore]
        public ICollection<Post> Posts { get; set; }
    }
}
