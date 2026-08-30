namespace ECommerce.Application.DTOs.Orders;

public record OrderResponse(
    int Id,
    int CustomerId,
    string Status,
    DateTime CreatedAt,
    decimal Subtotal,
    decimal DiscountAmount,
    decimal TaxAmount,
    decimal ShippingFee,
    decimal TotalAmount,
    IReadOnlyList<OrderItemResponse> Items);
