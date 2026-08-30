namespace ECommerce.Application.DTOs.Orders;

public record CheckoutOrderResponse(
    int OrderId,
    string Status,
    decimal Subtotal,
    decimal Discount,
    decimal Tax,
    decimal Shipping,
    decimal Total,
    string TransactionReference);
