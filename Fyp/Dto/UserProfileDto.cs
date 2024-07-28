namespace Fyp.Dto
{
    public class UserProfileDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string? ProfilePath { get; set; }
        public string? Bio { get; set; }
        public string Role { get; set; }
        public int TotalFollowers { get; set; }
        public int TotalFollowing { get; set; }
        public DateTime JoinDate { get; set; }
        
    }
}
