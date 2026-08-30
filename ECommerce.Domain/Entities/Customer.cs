using ECommerce.Domain.Common;
using ECommerce.Domain.Exceptions;

namespace ECommerce.Domain.Entities;

public sealed class Customer : Entity
{
    public string FullName { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public bool IsVip { get; private set; }

    private readonly List<Order> _orders = new();
    public IReadOnlyCollection<Order> Orders => _orders.AsReadOnly();

    private Customer() { }

    public Customer(string fullName, string email, bool isVip = false)
    {
        SetName(fullName);
        SetEmail(email);
        IsVip = isVip;
    }

    public void SetName(string fullName)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            throw new DomainException("Customer name cannot be empty.");
        FullName = fullName.Trim();
    }

    public void SetEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email) || !email.Contains('@'))
            throw new DomainException("A valid email address is required.");
        Email = email.Trim().ToLowerInvariant();
    }

    public void UpgradeToVip(decimal minimumSpendThreshold = 500m)
    {
        var totalSpent = _orders
            .Where(o => o.Status == Enums.OrderStatus.Paid)
            .Sum(o => o.TotalAmount);

        if (totalSpent < minimumSpendThreshold)
            throw new DomainException($"Customer does not qualify for VIP. Total spend {totalSpent:C} is below threshold {minimumSpendThreshold:C}.");

        IsVip = true;
    }
}
