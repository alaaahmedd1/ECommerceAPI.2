using ECommerce.Domain.Entities;

namespace ECommerce.Application.Interfaces.Repositories;

public interface IProductRepository
{
    Task<IReadOnlyList<Product>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Product>> GetByIdsAsync(IEnumerable<int> ids, CancellationToken cancellationToken = default);
    Task<bool> ExistsBySkuAsync(string sku, CancellationToken cancellationToken = default);
    Task AddAsync(Product product, CancellationToken cancellationToken = default);
    void Update(Product product);
    void Delete(Product product);
}
