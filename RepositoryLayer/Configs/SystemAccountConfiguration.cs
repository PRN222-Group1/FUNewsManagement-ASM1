using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RepositoryLayer.Entities;

namespace RepositoryLayer.Configs
{
    public class SystemAccountConfiguration : IEntityTypeConfiguration<SystemAccount>
    {
        public void Configure(EntityTypeBuilder<SystemAccount> builder)
        {
            builder.HasKey(e => e.Id);

            builder.ToTable("SystemAccount");

            builder.Property(e => e.Id).HasColumnName("AccountID");
            builder.Property(e => e.AccountEmail).HasMaxLength(70);
            builder.Property(e => e.AccountName).HasMaxLength(100);
            builder.Property(e => e.AccountPassword).HasMaxLength(70);
        }
    }
}
