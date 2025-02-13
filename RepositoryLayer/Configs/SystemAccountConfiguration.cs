using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RepositoryLayer.Data;

namespace RepositoryLayer.Configs
{
    public class SystemAccountConfiguration : IEntityTypeConfiguration<SystemAccount>
    {
        public void Configure(EntityTypeBuilder<SystemAccount> builder)
        {
            builder.HasKey(e => e.AccountId);

            builder.ToTable("SystemAccount");

            builder.Property(e => e.AccountId)
                .ValueGeneratedNever()
                .HasColumnName("AccountID");
            builder.Property(e => e.AccountEmail).HasMaxLength(70);
            builder.Property(e => e.AccountName).HasMaxLength(100);
            builder.Property(e => e.AccountPassword).HasMaxLength(70);
        }
    }
}
