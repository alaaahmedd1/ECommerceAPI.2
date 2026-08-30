using ECommerce.Application.DTOs.Orders;
using ECommerce.Domain.Entities;

namespace ECommerce.Application.Mappings;

public static class OrderMappings
{
    public static OrderResponse ToResponse(this Order order)
    {
        var items = order.Items.Select(i => new OrderItemResponse(
            i.ProductId,
            i.Product?.Name ?? string.Empty,
            i.Quantity,
            i.UnitPrice,
            i.LineTotal
        )).ToList().AsReadOnly();

        return new OrderResponse(
            order.Id,
            order.CustomerId,
            order.Status.ToString(),
            order.CreatedAt,
            order.Subtotal,
            order.DiscountAmount,
            order.TaxAmount,
            order.ShippingFee,
            order.TotalAmount,
            items
        );
    }
}
