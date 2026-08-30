using ECommerce.Application.DTOs.Customers;
using ECommerce.Application.Features.Customers.Commands.CreateCustomer;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers;

public sealed class CustomersController : BaseApiController
{
    private readonly IMediator _mediator;
    public CustomersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CustomerResponse>> GetById(int id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
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
        return Ok(new { message = "Customer upgraded to VIP successfully." });
    }
}