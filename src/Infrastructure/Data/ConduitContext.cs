using System.Reflection;
using Core.Entities;

namespace Infrastructure.Data;

public class ConduitContext(DbContextOptions<ConduitContext> options) : DbContext(options)
{
    public DbSet<UserEntity> Users => Set<UserEntity>();
    
    public DbSet<ArticleEntity> Articles => Set<ArticleEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
