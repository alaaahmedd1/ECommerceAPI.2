using ECommerce.Application.DTOs.Customers;
using ECommerce.Domain.Entities;

namespace ECommerce.Application.Mappings;

public static class CustomerMappings
{
    public static CustomerResponse ToResponse(this Customer customer) =>
        new(customer.Id, customer.FullName, customer.Email, customer.IsVip);
}
