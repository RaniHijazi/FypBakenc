namespace Fyp.Dto
{
    public class MessageDto2
    {
        public int MessageId { get; set; }
        public int SenderId { get; set; }
        public int? RecipientId { get; set; }
        public int? RoomId { get; set; }
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }
        public string RoomName { get; set; }
        public string ProfilePath { get; set; }
    }

}
