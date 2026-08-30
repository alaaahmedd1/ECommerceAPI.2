namespace ECommerce.Application.Common.Models;

public record PaymentResult(bool IsSuccess, string TransactionReference, string? ErrorMessage = null);
