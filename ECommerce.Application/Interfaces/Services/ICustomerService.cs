using ECommerce.Application.DTOs.Customers;

namespace ECommerce.Application.Interfaces.Services;

public interface ICustomerService
{
    Task<CustomerResponse> GetCustomerByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<CustomerResponse> RegisterCustomerAsync(CreateCustomerRequest request, CancellationToken cancellationToken = default);
    Task UpgradeToVipAsync(int customerId, CancellationToken cancellationToken = default);
}
