using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Fyp.Models
{
    public class Document
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; } = "Pending"; 
        public string Description { get; set; }
        public string UploadDate { get; set; }
        public string? Note { get; set; }
        public string? ImgUrl { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        [JsonIgnore]
        public User User { get; set; }

        
        [JsonIgnore]
        public DocumentApproval DocumentApproval { get; set; }
    }
}
