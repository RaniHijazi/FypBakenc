namespace Fyp.Dto
{
    public class UserProfileDto
    {
        public string FullName { get; set; }
        public string? ProfilePath { get; set; }
        public string? Bio { get; set; }
        public string Role { get; set; }
        public int TotalFollowers { get; set; }
        public int TotalFollowing { get; set; }
    }
}
