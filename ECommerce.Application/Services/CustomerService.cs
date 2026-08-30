using ECommerce.Application.Common.Exceptions;
using ECommerce.Application.DTOs.Customers;
using ECommerce.Application.Interfaces.Repositories;
using ECommerce.Application.Interfaces.Services;
using ECommerce.Application.Mappings;
using ECommerce.Domain.Entities;

namespace ECommerce.Application.Services;

public sealed class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CustomerService(ICustomerRepository customerRepository, IUnitOfWork unitOfWork)
    {
        _customerRepository = customerRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<CustomerResponse> GetCustomerByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var customer = await _customerRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException(nameof(Customer), id);

        return customer.ToResponse();
    }

    public async Task<CustomerResponse> RegisterCustomerAsync(CreateCustomerRequest request, CancellationToken cancellationToken = default)
    {
        if (await _customerRepository.ExistsByEmailAsync(request.Email, cancellationToken))
            throw new ValidationException($"Customer with email '{request.Email}' is already registered.");

        var customer = new Customer(request.FullName, request.Email, request.IsVip);
        await _customerRepository.AddAsync(customer, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return customer.ToResponse();
    }

    public async Task UpgradeToVipAsync(int customerId, CancellationToken cancellationToken = default)
    {
        var customer = await _customerRepository.GetByIdAsync(customerId, cancellationToken)
            ?? throw new NotFoundException(nameof(Customer), customerId);

        customer.UpgradeToVip();
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
