using ECommerce.Application.DTOs.Orders;
using ECommerce.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers;

public sealed class OrdersController : BaseApiController
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<OrderResponse>> GetById(int id, CancellationToken cancellationToken)
    {
        var order = await _orderService.GetOrderByIdAsync(id, cancellationToken);
        return Ok(order);
    }

    [HttpGet("customer/{customerId:int}")]
    public async Task<ActionResult<IReadOnlyList<OrderResponse>>> GetCustomerOrders(int customerId, CancellationToken cancellationToken)
    {
        var orders = await _orderService.GetCustomerOrdersAsync(customerId, cancellationToken);
        return Ok(orders);
    }

    [HttpPost("checkout")]
    public async Task<ActionResult<CheckoutOrderResponse>> Checkout([FromBody] CreateOrderRequest request, CancellationToken cancellationToken)
    {
        var result = await _orderService.CheckoutAsync(request, cancellationToken);
        return Ok(result);
    }

    [HttpPost("cancel/{id:int}")]
    public async Task<IActionResult> Cancel(int id, CancellationToken cancellationToken)
    {
        await _orderService.CancelOrderAsync(id, cancellationToken);
        return Ok(new { message = "Order cancelled successfully." });
    }
}
