using ECommerce.Application.DTOs.Products;
using ECommerce.Domain.Entities;

namespace ECommerce.Application.Mappings;

public static class ProductMappings
{
    public static ProductResponse ToResponse(this Product product) =>
        new(product.Id, product.Name, product.SKU, product.Price, product.StockQuantity);

    public static IReadOnlyList<ProductResponse> ToResponseList(this IEnumerable<Product> products) =>
        products.Select(ToResponse).ToList().AsReadOnly();
}
