using System.Text.Json.Serialization;

namespace Fyp.Models
{
    public class PreCommunity
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public string Description{ get; set; }
        [JsonIgnore]
        public ICollection<PreSubCommunity> PreSubCommunities { get; set; }
        [JsonIgnore]
        public ICollection<User> Users { get; set; }
        public ICollection<Post> Posts { get; set; }
    }
}
