using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Persistence.Configurations;

public sealed class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders");
        builder.HasKey(o => o.Id);

        builder.Property(o => o.Subtotal).HasPrecision(18, 2);
        builder.Property(o => o.DiscountAmount).HasPrecision(18, 2);
        builder.Property(o => o.TaxAmount).HasPrecision(18, 2);
        builder.Property(o => o.ShippingFee).HasPrecision(18, 2);
        builder.Property(o => o.TotalAmount).HasPrecision(18, 2);

        builder.HasMany(o => o.Items)
            .WithOne()
            .HasForeignKey(i => i.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(o => o.Payment)
            .WithOne()
            .HasForeignKey<Payment>(p => p.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasData(
            new
            {
                Id = 1,
                CustomerId = 1,
                CreatedAt = new DateTime(2026, 1, 15, 10, 30, 0, DateTimeKind.Utc),
                Status = Domain.Enums.OrderStatus.Paid,
                Subtotal = 120.00m,
                DiscountAmount = 18.00m,
                TaxAmount = 14.28m,
                ShippingFee = 75.00m,
                TotalAmount = 191.28m
            },
            new
            {
                Id = 2,
                CustomerId = 2,
                CreatedAt = new DateTime(2026, 2, 1, 14, 0, 0, DateTimeKind.Utc),
                Status = Domain.Enums.OrderStatus.Pending,
                Subtotal = 45.50m,
                DiscountAmount = 0.00m,
                TaxAmount = 6.37m,
                ShippingFee = 75.00m,
                TotalAmount = 126.87m
            }
        );
    }
}
