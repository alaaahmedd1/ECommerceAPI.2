using ECommerce.Application.DTOs.Products;
using ECommerce.Application.Interfaces.Repositories;
using ECommerce.Application.Interfaces.Services;
using ECommerce.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.Products.Commands.CreateProduct
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ProductResponse>
    {
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateProductCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ProductResponse> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            if (await _productRepository.ExistsBySkuAsync(command.SKU, cancellationToken))
            {
                throw new ValidationException($"Product with SKU '{command.SKU}' already exists.");
            }

            var product = new Product(command.Name, command.SKU, command.Price, command.StockQuantity);

            await _productRepository.AddAsync(product, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return product.ToResponse();
        }
    }
}
