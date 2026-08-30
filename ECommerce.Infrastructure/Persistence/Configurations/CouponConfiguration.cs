using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Persistence.Configurations;

public sealed class CouponConfiguration : IEntityTypeConfiguration<Coupon>
{
    public void Configure(EntityTypeBuilder<Coupon> builder)
    {
        builder.ToTable("Coupons");
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Code).IsRequired().HasMaxLength(50);
        builder.HasIndex(c => c.Code).IsUnique();
        builder.Property(c => c.DiscountPercentage).HasPrecision(5, 2);

        builder.HasData(
            new { Id = 1, Code = "WELCOME10", DiscountPercentage = 10.00m, IsActive = true },
            new { Id = 2, Code = "SUMMER20", DiscountPercentage = 20.00m, IsActive = true },
            new { Id = 3, Code = "EXPIRED50", DiscountPercentage = 50.00m, IsActive = false }
        );
    }
}
