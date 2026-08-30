using ECommerce.Domain.Common;
using ECommerce.Domain.Exceptions;

namespace ECommerce.Domain.Entities;

public sealed class Product : Entity
{
    public string Name { get; private set; } = string.Empty;
    public string SKU { get; private set; } = string.Empty;
    public decimal Price { get; private set; }
    public int StockQuantity { get; private set; }

    private Product() { }

    public Product(string name, string sku, decimal price, int stockQuantity)
    {
        UpdateDetails(name, sku, price);
        SetInitialStock(stockQuantity);
    }

    public void UpdateDetails(string name, string sku, decimal price)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Product name cannot be empty.");
        if (string.IsNullOrWhiteSpace(sku))
            throw new DomainException("Product SKU cannot be empty.");
        if (price <= 0)
            throw new DomainException("Product price must be greater than zero.");

        Name = name.Trim();
        SKU = sku.Trim().ToUpperInvariant();
        Price = Math.Round(price, 2);
    }

    private void SetInitialStock(int quantity)
    {
        if (quantity < 0)
            throw new DomainException("Stock quantity cannot be negative.");
        StockQuantity = quantity;
    }

    public void DeductStock(int quantity)
    {
        if (quantity <= 0)
            throw new DomainException("Quantity to deduct must be greater than zero.");
        if (StockQuantity < quantity)
            throw new InsufficientStockException(Name, StockQuantity, quantity);

        StockQuantity -= quantity;
    }

    public void Restock(int quantity)
    {
        if (quantity <= 0)
            throw new DomainException("Restock quantity must be greater than zero.");

        StockQuantity += quantity;
    }
}
