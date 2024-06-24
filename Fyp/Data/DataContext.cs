
using Fyp.Models;
using Microsoft.EntityFrameworkCore;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }
    public DbSet<User> users { get; set; }
    public DbSet<Community> communities { get; set; }
    public DbSet<SubCommunity> sub_communities { get; set; }
    public DbSet<UserSubCommunity> user_sub_communities { get; set; }
    public DbSet<Document> documents { get; set; }
    public DbSet<Like> likes { get; set; }
    public DbSet<Comment> comments { get; set; }
    public DbSet<Post> posts { get; set; }
    public DbSet<ChatRoom> chat_rooms { get; set; }
    public DbSet<Message> messages { get; set; }
    public DbSet<UserChatRoom> user_chat_rooms { get; set; }
    public DbSet<Follow> Follows { get; set; }
    public DbSet<Faculty> faculties { get; set; }
    public DbSet<Major> majors { get; set; }
    public DbSet<Corse> corses { get; set; }
    public DbSet<Story> stories { get; set; }

    public DbSet<DocumentApproval> documents_approval { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<UserChatRoom>()
            .HasKey(ucr => ucr.Id);

        modelBuilder.Entity<UserChatRoom>()
            .HasOne(ucr => ucr.User)
            .WithMany(u => u.UserChatRooms)
            .HasForeignKey(ucr => ucr.UserId);

        modelBuilder.Entity<UserChatRoom>()
            .HasOne(ucr => ucr.Room)
            .WithMany(r => r.UserChatRooms)
            .HasForeignKey(ucr => ucr.RoomId);


        modelBuilder.Entity<UserSubCommunity>()
            .HasKey(usc => usc.Id);

        modelBuilder.Entity<UserSubCommunity>()
            .HasOne(ucr => ucr.User)
            .WithMany(u => u.UserSubCommunities)
            .HasForeignKey(ucr => ucr.UserId);

        modelBuilder.Entity<UserSubCommunity>()
            .HasOne(ucr => ucr.SubCommunity)
            .WithMany(r => r.UserSubCommunities)
            .HasForeignKey(ucr => ucr.SubCommunityId);

        modelBuilder.Entity<Message>()
            .HasOne(m => m.Sender)
            .WithMany()
            .HasForeignKey(m => m.SenderId)
               .OnDelete(DeleteBehavior.Restrict);


        modelBuilder.Entity<Message>()
            .HasOne(m => m.Recipient)
            .WithMany()
            .HasForeignKey(m => m.RecipientId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Post>()
            .HasOne(p => p.Community)
            .WithMany(pc => pc.Posts)
            .HasForeignKey(p => p.CommunityId)
             .OnDelete(DeleteBehavior.Restrict);


        modelBuilder.Entity<Post>()
            .HasOne(p => p.SubCommunity)
            .WithMany(psc => psc.Posts)
            .HasForeignKey(p => p.SubCommunityId)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<UserSubCommunity>()
            .HasOne(usc => usc.User)
            .WithMany(u => u.UserSubCommunities)
            .HasForeignKey(usc => usc.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Comment>()
            .HasOne(c => c.User)
            .WithMany(u => u.Comments)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Follow>()
                .HasKey(f => f.Id);

        modelBuilder.Entity<Follow>()
            .HasOne(f => f.Follower)
            .WithMany(u => u.Followings)
            .HasForeignKey(f => f.FollowerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Follow>()
            .HasOne(f => f.Followed)
            .WithMany(u => u.Followers)
            .HasForeignKey(f => f.FollowedId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Document>()
           .HasOne(d => d.DocumentApproval)
           .WithOne(da => da.Document)
           .HasForeignKey<DocumentApproval>(da => da.DocumentId)
           .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<DocumentApproval>()
            .HasOne(da => da.AdminUser)
            .WithMany(u => u.DocumentApprovals)
            .HasForeignKey(da => da.ApprovedById)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
