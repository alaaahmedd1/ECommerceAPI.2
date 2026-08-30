using ECommerce.Domain.Common;
using ECommerce.Domain.Enums;
using ECommerce.Domain.Exceptions;

namespace ECommerce.Domain.Entities;

public sealed class Payment : Entity
{
    public int OrderId { get; private set; }
    public decimal Amount { get; private set; }
    public DateTime PaymentDate { get; private set; }
    public string TransactionReference { get; private set; } = string.Empty;
    public PaymentStatus Status { get; private set; }

    private Payment() { }

    public Payment(int orderId, decimal amount, string transactionReference, PaymentStatus status)
    {
        if (orderId <= 0)
            throw new DomainException("Invalid Order ID.");
        if (amount <= 0)
            throw new DomainException("Payment amount must be greater than zero.");
        if (string.IsNullOrWhiteSpace(transactionReference))
            throw new DomainException("Transaction reference is required.");

        OrderId = orderId;
        Amount = Math.Round(amount, 2);
        TransactionReference = transactionReference;
        Status = status;
        PaymentDate = DateTime.UtcNow;
    }
}
