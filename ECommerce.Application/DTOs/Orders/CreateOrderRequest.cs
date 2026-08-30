namespace ECommerce.Application.DTOs.Orders;

public record CreateOrderRequest(int CustomerId, List<OrderItemRequest> Items, string? CouponCode);
