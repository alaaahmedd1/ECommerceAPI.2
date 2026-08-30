namespace ECommerce.Application.DTOs.Products;

public record ProductResponse(int Id, string Name, string SKU, decimal Price, int StockQuantity);
