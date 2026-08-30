using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Persistence.Configurations;

public sealed class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customers");
        builder.HasKey(c => c.Id);

        builder.Property(c => c.FullName).IsRequired().HasMaxLength(100);
        builder.Property(c => c.Email).IsRequired().HasMaxLength(150);
        builder.HasIndex(c => c.Email).IsUnique();

        builder.HasMany(c => c.Orders)
            .WithOne(o => o.Customer)
            .HasForeignKey(o => o.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasData(
            new { Id = 1, FullName = "Sarah Connor", Email = "sarah.connor@sky.net", IsVip = true },
            new { Id = 2, FullName = "John Doe", Email = "john.doe@example.com", IsVip = false },
            new { Id = 3, FullName = "Jane Smith", Email = "jane.smith@example.com", IsVip = false }
        );
    }
}
