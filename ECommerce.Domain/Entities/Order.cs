using ECommerce.Domain.Common;
using ECommerce.Domain.Enums;
using ECommerce.Domain.Exceptions;

namespace ECommerce.Domain.Entities;

public sealed class Order : Entity
{
    public int CustomerId { get; private set; }
    public Customer? Customer { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public OrderStatus Status { get; private set; }
    public decimal Subtotal { get; private set; }
    public decimal DiscountAmount { get; private set; }
    public decimal TaxAmount { get; private set; }
    public decimal ShippingFee { get; private set; }
    public decimal TotalAmount { get; private set; }

    private readonly List<OrderItem> _items = new();
    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();

    public Payment? Payment { get; private set; }

    private Order() { }

    public Order(int customerId)
    {
        if (customerId <= 0)
            throw new DomainException("Invalid Customer ID.");

        CustomerId = customerId;
        CreatedAt = DateTime.UtcNow;
        Status = OrderStatus.Pending;
    }

    public void AddItem(int productId, int quantity, decimal unitPrice)
    {
        if (Status != OrderStatus.Pending)
            throw new InvalidOrderStateException("Cannot modify items on a non-pending order.");

        var existingItem = _items.FirstOrDefault(i => i.ProductId == productId);
        if (existingItem != null)
            throw new DomainException("Product already exists in the order. Update quantity instead.");

        _items.Add(new OrderItem(productId, quantity, unitPrice));
    }

    public void CalculateTotals(bool isCustomerVip, Coupon? coupon = null, decimal taxRate = 0.14m, decimal freeShippingThreshold = 1000m, decimal standardShippingFee = 75m)
    {
        if (!_items.Any())
            throw new DomainException("Cannot calculate totals for an empty order.");

        Subtotal = _items.Sum(i => i.LineTotal);

        decimal discount = 0m;
        if (isCustomerVip)
            discount += Math.Round(Subtotal * 0.15m, 2);

        if (coupon != null && coupon.IsActive)
            discount += coupon.CalculateDiscount(Subtotal);

        DiscountAmount = Math.Min(Subtotal, discount);
        var netAmount = Subtotal - DiscountAmount;

        TaxAmount = Math.Round(netAmount * taxRate, 2);
        ShippingFee = netAmount >= freeShippingThreshold ? 0m : standardShippingFee;
        TotalAmount = netAmount + TaxAmount + ShippingFee;
    }

    public void MarkAsPaid(string transactionReference)
    {
        if (Status == OrderStatus.Paid)
            throw new InvalidOrderStateException("Order is already paid.");
        if (Status == OrderStatus.Cancelled)
            throw new InvalidOrderStateException("Cannot pay a cancelled order.");
        if (TotalAmount <= 0)
            throw new InvalidOrderStateException("Order totals must be calculated prior to payment.");

        Status = OrderStatus.Paid;
        Payment = new Payment(Id, TotalAmount, transactionReference, PaymentStatus.Success);
    }

    public void Cancel()
    {
        if (Status == OrderStatus.Cancelled)
            throw new InvalidOrderStateException("Order is already cancelled.");

        Status = OrderStatus.Cancelled;
    }
}
