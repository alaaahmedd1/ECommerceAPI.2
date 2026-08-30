using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using FluentValidation;

namespace ECommerce.Application.Features.Products.Commands.CreateProduct
{
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("Please enter product's name.")
                .MaximumLength(50);
            RuleFor(p => p.Price)
                .GreaterThan(0);
            RuleFor(p => p.StockQuantity)
                .GreaterThanOrEqual(0);
        }
    }
}
