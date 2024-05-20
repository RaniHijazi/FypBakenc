using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
namespace Fyp.Models
{
    public class Corse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Credits { get; set; }
        [ForeignKey("major")]
        public int MajorId { get; set; }

        [JsonIgnore]
        public Major major { get; set; }

    }
}
