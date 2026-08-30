using ECommerce.Application.DTOs.Customers;
using ECommerce.Application.Features.Customers.Commands.CreateCustomer;
using ECommerce.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers;

public sealed class CustomersController : BaseApiController
{
    private readonly ICustomerService _customerService;

    public CustomersController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CustomerResponse>> GetById(int id, CancellationToken cancellationToken)
    {
        var customer = await _customerService.GetCustomerByIdAsync(id, cancellationToken);
        return Ok(customer);
    }

    [HttpPost]
    public async Task<ActionResult<CustomerResponse>> Create([FromBody] CreateCustomerCommand command, CancellationToken cancellationToken)
    {
        var created = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPost("{id:int}/upgrade-vip")]
    public async Task<IActionResult> UpgradeVip(int id, CancellationToken cancellationToken)
    {
        await _customerService.UpgradeToVipAsync(id, cancellationToken);
        return Ok(new { message = "Customer upgraded to VIP successfully." });
    }
}
