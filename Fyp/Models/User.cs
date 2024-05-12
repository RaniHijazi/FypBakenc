using System.Text.Json.Serialization;

namespace Fyp.Models
{
    public class User
    {
        public int Id { get; set; }  
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string? ProfilePath { get; set; }
        public string? Bio { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public string? School { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Role { get; set; }
        public DateTime JoinDate { get; set; }  

        public int CommunityId { get; set; }
        [JsonIgnore]
        public PreCommunity PreCommunity { get; set; }
        [JsonIgnore]
        public ICollection<UserSubCommunity> UserSubCommunities { get; set; }
        [JsonIgnore]
        public ICollection<Post> Posts { get; set; }
        [JsonIgnore]
        public ICollection<Comment> Comments { get; set; }
        [JsonIgnore]
        public ICollection<Like> Likes { get; set; }
        [JsonIgnore]
        public ICollection<Message> SentMessages { get; set; }
        [JsonIgnore]
        public ICollection<UserChatRoom> UserChatRooms { get; set; }
        [JsonIgnore]
        public ICollection<Document> Documents { get; set; }





    }
}
