namespace Fyp.Dto
{
    public class GetUserStoriesDto
    {
        public int UserId { get; set; }
        public string UserFullName { get; set; }
        public string UserProfileImageUrl { get; set; }
        public List<GetStoryDto> Stories { get; set; }
    }
}
