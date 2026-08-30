using ECommerce.Application.Common.Exceptions;
using ECommerce.Application.DTOs.Customers;
using ECommerce.Application.Interfaces.Repositories;
using ECommerce.Application.Interfaces.Services;
using ECommerce.Application.Mappings;
using ECommerce.Domain.Entities;
using MediatR;

namespace ECommerce.Application.Features.Customers.Commands.CreateCustomer;

public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, CustomerResponse>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateCustomerCommandHandler(ICustomerRepository customerRepository, IUnitOfWork unitOfWork)
    {
        _customerRepository = customerRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<CustomerResponse> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        if (await _customerRepository.ExistsByEmailAsync(request.Email, cancellationToken))
        {
            throw new ValidationException($"Customer with email '{request.Email}' is already registered.");
        }
        
        var customer = new Customer(request.FullName, request.Email, request.IsVip);

        await _customerRepository.AddAsync(customer, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return customer.ToResponse();
    }
}