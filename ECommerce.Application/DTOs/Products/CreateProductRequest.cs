namespace ECommerce.Application.DTOs.Products;

public record CreateProductRequest(string Name, string SKU, decimal Price, int StockQuantity);
