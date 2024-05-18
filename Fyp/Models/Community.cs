using System.Text.Json.Serialization;

namespace Fyp.Models
{
    public class Community
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public string Description{ get; set; }
        [JsonIgnore]
        public ICollection<SubCommunity> SubCommunities { get; set; }
        [JsonIgnore]
        public ICollection<User> Users { get; set; }
        public ICollection<Post> Posts { get; set; }
    }
}
