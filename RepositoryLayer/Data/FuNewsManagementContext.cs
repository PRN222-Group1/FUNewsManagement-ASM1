using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Entities;
using System.Reflection;

namespace RepositoryLayer.Data;

public partial class FuNewsManagementContext : DbContext
{
    public FuNewsManagementContext()
    {
    }

    public FuNewsManagementContext(DbContextOptions<FuNewsManagementContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<NewsArticle> NewsArticles { get; set; }

    public virtual DbSet<SystemAccount> SystemAccounts { get; set; }

    public virtual DbSet<Tag> Tags { get; set; }

    public virtual DbSet<NewsTag> NewsTags { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
