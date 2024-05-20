namespace Fyp.Models
{
    public class Faculty
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImgUrl { get; set; }
        public ICollection<Major> majors { get; set; }
    }
}
