namespace ECommerce.Domain.Exceptions;

public sealed class InsufficientStockException : DomainException
{
    public InsufficientStockException(string productName, int available, int requested)
        : base($"Insufficient stock for product '{productName}'. Available: {available}, Requested: {requested}")
    {
    }
}
