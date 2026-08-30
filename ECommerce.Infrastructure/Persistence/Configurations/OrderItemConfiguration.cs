using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Persistence.Configurations;

public sealed class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable("OrderItems");
        builder.HasKey(i => i.Id);

        builder.Property(i => i.UnitPrice).HasPrecision(18, 2);

        builder.HasOne(i => i.Product)
            .WithMany()
            .HasForeignKey(i => i.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasData(
            new { Id = 1, OrderId = 1, ProductId = 1, Quantity = 1, UnitPrice = 120.00m },
            new { Id = 2, OrderId = 2, ProductId = 2, Quantity = 1, UnitPrice = 45.50m }
        );
    }
}
