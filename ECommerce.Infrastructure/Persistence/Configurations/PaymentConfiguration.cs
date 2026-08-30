using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Persistence.Configurations;

public sealed class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.ToTable("Payments");
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Amount).HasPrecision(18, 2);
        builder.Property(p => p.TransactionReference).IsRequired().HasMaxLength(100);

        builder.HasData(
            new
            {
                Id = 1,
                OrderId = 1,
                Amount = 191.28m,
                PaymentDate = new DateTime(2026, 1, 15, 10, 35, 0, DateTimeKind.Utc),
                TransactionReference = "TX-REF-10001",
                Status = Domain.Enums.PaymentStatus.Success
            }
        );
    }
}
