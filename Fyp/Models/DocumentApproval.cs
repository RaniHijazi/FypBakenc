using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Fyp.Models
{
    public class DocumentApproval
    {
        public int Id { get; set; }

        [ForeignKey("Document")]
        public int DocumentId { get; set; }

        [ForeignKey("AdminUser")]
        public int ApprovedById { get; set; } 

        public DateTime ApprovalDate { get; set; }
        public string Status { get; set; } 

        [JsonIgnore]
        public Document Document { get; set; }

        [JsonIgnore]
        public User AdminUser { get; set; } 
    }
}
