using Chibegir.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Chibegir.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<Source> Sources { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure Product entity
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Title).HasMaxLength(250).IsRequired();
            entity.Property(e => e.ProductUrl).HasMaxLength(500).IsRequired();
            entity.Property(e => e.Html).HasColumnType("nvarchar(max)");
            entity.Property(e => e.Price).HasColumnType("decimal(18,2)");
            entity.Property(e => e.LastUpdate).IsRequired();
            entity.Property(e => e.CreatedOn).IsRequired();
            
            // Configure relationship with Source
            entity.HasOne(e => e.Source)
                  .WithMany()
                  .HasForeignKey(e => e.SourceId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Configure Source entity
        modelBuilder.Entity<Source>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.SourceName).IsRequired();
            entity.Property(e => e.SourceBaseAddress).IsRequired();
            entity.Property(e => e.CreatedOn).IsRequired();
        });
    }
}
