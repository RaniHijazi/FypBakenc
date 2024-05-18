using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fyp.Models
{
    public class Follow
    {
        [Key]
        public int Id { get; set; }

        public int FollowerId { get; set; }
        [ForeignKey("FollowerId")]
        public User Follower { get; set; }

        public int FollowedId { get; set; }
        [ForeignKey("FollowedId")]
        public User Followed { get; set; }

        public DateTime FollowedDate { get; set; }
    }
}
