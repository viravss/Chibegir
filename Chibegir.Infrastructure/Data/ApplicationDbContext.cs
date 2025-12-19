using Chibegir.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Chibegir.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Product> Product { get; set; }
    public DbSet<ProductSource> ProductSource { get; set; }
    public DbSet<Source> Source { get; set; }
    public DbSet<ProductLog> ProductLog { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        DbContextConfiguration.ProductConfigurations(modelBuilder);
        DbContextConfiguration.SourceConfigurations(modelBuilder);
        DbContextConfiguration.ProductSourceConfigurations(modelBuilder);
        DbContextConfiguration.ProductLogConfigurations(modelBuilder);
    }
}
