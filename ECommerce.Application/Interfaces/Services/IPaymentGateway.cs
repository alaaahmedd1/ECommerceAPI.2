using ECommerce.Application.Common.Models;

namespace ECommerce.Application.Interfaces.Services;

public interface IPaymentGateway
{
    Task<PaymentResult> ChargeAsync(string customerEmail, decimal amount, CancellationToken cancellationToken = default);
}
