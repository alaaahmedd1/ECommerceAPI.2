using ECommerce.Domain.Common;
using ECommerce.Domain.Exceptions;

namespace ECommerce.Domain.Entities;

public sealed class Coupon : Entity
{
    public string Code { get; private set; } = string.Empty;
    public decimal DiscountPercentage { get; private set; }
    public bool IsActive { get; private set; }

    private Coupon() { }

    public Coupon(string code, decimal discountPercentage, bool isActive = true)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new DomainException("Coupon code cannot be empty.");
        if (discountPercentage <= 0 || discountPercentage > 100)
            throw new DomainException("Discount percentage must be between 0.01 and 100.");

        Code = code.Trim().ToUpperInvariant();
        DiscountPercentage = discountPercentage;
        IsActive = isActive;
    }

    public decimal CalculateDiscount(decimal amount)
    {
        if (!IsActive || amount <= 0) return 0m;
        return Math.Round(amount * (DiscountPercentage / 100m), 2);
    }
}
