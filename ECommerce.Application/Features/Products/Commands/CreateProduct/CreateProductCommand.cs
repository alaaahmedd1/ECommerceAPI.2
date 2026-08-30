using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace ECommerce.Application.Features.Products.Commands.CreateProduct
{
   public record CreateProductCommand(string Name, string SKU, decimal Price, int StokeQuantity) : IRequest<ProductResponse>;
}
