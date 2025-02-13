using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RepositoryLayer.Data;

namespace RepositoryLayer.Configs
{
    public class TagConfiguration : IEntityTypeConfiguration<Tag>
    {
        public void Configure(EntityTypeBuilder<Tag> builder)
        {
            builder.HasKey(e => e.TagId).HasName("PK_HashTag");

            builder.ToTable("Tag");

            builder.Property(e => e.TagId)
                .ValueGeneratedNever()
                .HasColumnName("TagID");
            builder.Property(e => e.Note).HasMaxLength(400);
            builder.Property(e => e.TagName).HasMaxLength(50);
        }
    }
}
