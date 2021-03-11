using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using YAFF.Core.Entities;
using YAFF.Core.Entities.Identity;

namespace YAFF.Data
{
    public class ForumDbContext : IdentityDbContext<User, Role, int>
    {
        public DbSet<Post> Posts { get; set; }
        public DbSet<PostPreview> PostPreviews { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<UserProfile> Profiles { get; set; }
        public DbSet<PostTag> PostTags { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<PostLike> PostLikes { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<PrivateChat> PrivateChats { get; set; }
        public DbSet<GroupChat> GroupChats { get; set; }
        public DbSet<ChatUser> ChatUsers { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }


        public ForumDbContext(DbContextOptions<ForumDbContext> option) : base(option)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(typeof(ForumDbContext).Assembly);
            base.OnModelCreating(builder);
        }
    }
}