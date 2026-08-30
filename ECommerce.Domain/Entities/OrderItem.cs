using ECommerce.Domain.Common;
using ECommerce.Domain.Exceptions;

namespace ECommerce.Domain.Entities;

public sealed class OrderItem : Entity
{
    public int OrderId { get; private set; }
    public int ProductId { get; private set; }
    public Product? Product { get; private set; }
    public int Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }

    private OrderItem() { }

    public OrderItem(int productId, int quantity, decimal unitPrice)
    {
        if (productId <= 0)
            throw new DomainException("Invalid Product ID.");
        if (quantity <= 0)
            throw new DomainException("Quantity must be at least 1.");
        if (unitPrice <= 0)
            throw new DomainException("Unit price must be greater than zero.");

        ProductId = productId;
        Quantity = quantity;
        UnitPrice = Math.Round(unitPrice, 2);
    }

    public decimal LineTotal => Quantity * UnitPrice;
}
