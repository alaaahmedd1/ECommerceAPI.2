using ECommerce.Application.Common.Models;
using ECommerce.Application.Interfaces.Services;

namespace ECommerce.Infrastructure.Services;

public sealed class FakePaymentGateway : IPaymentGateway
{
    public Task<PaymentResult> ChargeAsync(string customerEmail, decimal amount, CancellationToken cancellationToken = default)
    {
        if (amount > 50000m)
            return Task.FromResult(new PaymentResult(false, string.Empty, "Payment exceeds maximum allowed limit ($50,000.00)."));

        var reference = $"TX-ONION-{Guid.NewGuid().ToString()[..8].ToUpperInvariant()}";
        return Task.FromResult(new PaymentResult(true, reference));
    }
}
