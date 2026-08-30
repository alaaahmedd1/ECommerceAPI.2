namespace ECommerce.Application.DTOs.Products;

public record UpdateProductRequest(string Name, string SKU, decimal Price, int StockQuantity);
