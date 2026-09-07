using ECommerce.Application.DTOs.Customers;
using MediatR;

namespace ECommerce.Application.Features.Customers.Commands.CreateCustomer;

public record CreateCustomerCommand(string FullName, string Email, bool IsVip) : IRequest<CustomerResponse>;
