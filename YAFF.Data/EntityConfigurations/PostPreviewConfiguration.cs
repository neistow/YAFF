using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YAFF.Core.Entities;

namespace YAFF.Data.EntityConfigurations
{
    public class PostPreviewConfiguration : IEntityTypeConfiguration<PostPreview>
    {
        public void Configure(EntityTypeBuilder<PostPreview> builder)
        {
            builder.HasOne(pp => pp.Post)
                .WithOne(p => p.Preview)
                .HasForeignKey<PostPreview>(p => p.PostId);
        }
    }
}