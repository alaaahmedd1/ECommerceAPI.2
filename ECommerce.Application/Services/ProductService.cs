using ECommerce.Application.Common.Exceptions;
using ECommerce.Application.DTOs.Products;
using ECommerce.Application.Interfaces.Repositories;
using ECommerce.Application.Interfaces.Services;
using ECommerce.Application.Mappings;
using ECommerce.Domain.Entities;

namespace ECommerce.Application.Services;

public sealed class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ProductService(IProductRepository productRepository, IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<IReadOnlyList<ProductResponse>> GetAllProductsAsync(CancellationToken cancellationToken = default)
    {
        var products = await _productRepository.GetAllAsync(cancellationToken);
        return products.ToResponseList();
    }

    public async Task<ProductResponse> GetProductByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var product = await _productRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException(nameof(Product), id);

        return product.ToResponse();
    }

    public async Task<ProductResponse> CreateProductAsync(CreateProductRequest request, CancellationToken cancellationToken = default)
    {
        if (await _productRepository.ExistsBySkuAsync(request.SKU, cancellationToken))
            throw new ValidationException($"Product with SKU '{request.SKU}' already exists.");

        var product = new Product(request.Name, request.SKU, request.Price, request.StockQuantity);
        await _productRepository.AddAsync(product, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return product.ToResponse();
    }

    public async Task UpdateProductAsync(int id, UpdateProductRequest request, CancellationToken cancellationToken = default)
    {
        var product = await _productRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException(nameof(Product), id);

        product.UpdateDetails(request.Name, request.SKU, request.Price);
        _productRepository.Update(product);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteProductAsync(int id, CancellationToken cancellationToken = default)
    {
        var product = await _productRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException(nameof(Product), id);

        _productRepository.Delete(product);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
