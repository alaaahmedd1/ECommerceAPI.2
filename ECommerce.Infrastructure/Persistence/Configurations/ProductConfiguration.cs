using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Persistence.Configurations;

public sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name).IsRequired().HasMaxLength(150);
        builder.Property(p => p.SKU).IsRequired().HasMaxLength(50);
        builder.HasIndex(p => p.SKU).IsUnique();
        builder.Property(p => p.Price).HasPrecision(18, 2);
        builder.Property(p => p.StockQuantity).IsRequired();

        builder.HasData(
            new { Id = 1, Name = "Mechanical Keyboard", SKU = "TECH-MK-01", Price = 120.00m, StockQuantity = 25 },
            new { Id = 2, Name = "Wireless Ergonomic Mouse", SKU = "TECH-WM-02", Price = 45.50m, StockQuantity = 40 },
            new { Id = 3, Name = "UltraWide Monitor 34"", SKU = "DISP-UW-03", Price = 650.00m, StockQuantity = 8 },
            new { Id = 4, Name = "USB-C Multiport Dock", SKU = "ACC-DK-04", Price = 85.00m, StockQuantity = 15 },
            new { Id = 5, Name = "Noise Cancelling Headphones", SKU = "AUD-NC-05", Price = 220.00m, StockQuantity = 12 }
        );
    }
}
