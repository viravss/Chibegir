using Chibegir.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Chibegir.Infrastructure.Data;

public static class DbContextConfiguration
{
    public static void ProductConfigurations(ModelBuilder modelBuilder)
    {
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

            // Configure relationship with Category
            entity.HasOne(e => e.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            //// Configure relationship with Source
            //entity.HasOne(e => e.Source)
            //    .WithMany()
            //    .HasForeignKey(e => e.SourceId)
            //    .OnDelete(DeleteBehavior.Restrict);
        });
    }

    public static void ProductSourceConfigurations(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProductSource>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();

            entity.HasOne(e => e.Source)
                .WithMany()
                .HasForeignKey(e => e.SourceId)
                .OnDelete(DeleteBehavior.Restrict);



            entity.HasOne(e => e.Product)
                .WithMany()
                .HasForeignKey(e => e.ProductId)
                .OnDelete(DeleteBehavior.Restrict);


        });
    }


    public static void SourceConfigurations(ModelBuilder modelBuilder)
    {
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

    public static void ProductLogConfigurations(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProductLog>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Action).HasMaxLength(50).IsRequired();
            entity.Property(e => e.CreatedOn).IsRequired();

            entity.HasOne(e => e.Product)
                .WithMany()
                .HasForeignKey(e => e.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Source)
                .WithMany()
                .HasForeignKey(e => e.SourceId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    public static void CategoryConfigurations(ModelBuilder modelBuilder)
    {
        // Configure Category entity
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Name).HasMaxLength(250).IsRequired();
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.CreatedOn).IsRequired();
        });
    }

}

