namespace ECommerce.Application.DTOs.Orders;

public record OrderItemResponse(int ProductId, string ProductName, int Quantity, decimal UnitPrice, decimal LineTotal);
