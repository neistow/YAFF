using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YAFF.Core.Entities;

namespace YAFF.Data.EntityConfigurations
{
    public class ChatUserConfiguration : IEntityTypeConfiguration<ChatUser>
    {
        public void Configure(EntityTypeBuilder<ChatUser> builder)
        {
            builder.HasKey(cu => new {cu.ChatId, cu.UserId});

            builder.HasOne(cu => cu.Chat)
                .WithMany(c => c.Users)
                .HasForeignKey(c => c.ChatId);

            builder.HasOne(cu => cu.User)
                .WithMany(u => u.Chats)
                .HasForeignKey(c => c.UserId);
        }
    }
}