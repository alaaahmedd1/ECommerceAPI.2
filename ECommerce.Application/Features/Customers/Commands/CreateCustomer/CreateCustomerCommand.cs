using ECommerce.Application.DTOs.Customers;
using MediatR;                               
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.Customers.Commands.CreateCustomer
{
    public record CreatesCustomerCommand(string Fullname, string Email, bool isVip) : IRequest<CustomerResponse>
}
