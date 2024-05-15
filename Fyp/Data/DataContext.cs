using Fyp.Models;
using Microsoft.EntityFrameworkCore;

    public class DataContext : DbContext
    {
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }
    public DbSet<User> users { get; set; }
    public DbSet<PreCommunity> pre_communities  { get; set; }
    public DbSet<PreSubCommunity> pre_sub_communities { get; set; }
    public DbSet<UserSubCommunity> user_sub_communities { get; set; }
    public DbSet<Document> documents { get; set; }
    public DbSet<Like> likes { get; set; }
    public DbSet<Comment> comments { get; set; }
    public DbSet<Post> posts { get; set; }
    public DbSet<ChatRoom> chat_rooms { get; set; }  
    public DbSet<Message> messages { get; set; }
    public DbSet<UserChatRoom>user_chat_rooms { get; set; }
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
            .HasOne(p => p.PreCommunity)             
            .WithMany(pc => pc.Posts)                
            .HasForeignKey(p => p.PreCommunityId)  
             .OnDelete(DeleteBehavior.Restrict);     


        modelBuilder.Entity<Post>()
            .HasOne(p => p.PreSubCommunity)          
            .WithMany(psc => psc.Posts)              
            .HasForeignKey(p => p.PreSubCommunityId) 
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
    }
}

