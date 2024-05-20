using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Fyp.Models
{
    public class Major
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }
        public string Description{ get; set; }
        public string ImgUrl { get; set; }
        public ICollection<Corse> corses { get; set; }
        [ForeignKey("faculty")]
        public int FacultyId { get; set; }
        [JsonIgnore]
        public Faculty faculty { get; set; }
    }
}
