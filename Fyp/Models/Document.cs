using System.Text.Json.Serialization;

namespace Fyp.Models
{
    public class Document
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string content { get; set; }
        public string UploadDate { get; set; }
        public string Status { get; set; }
        
        [JsonIgnore]
        public User User{ get; set; }
    }
}
