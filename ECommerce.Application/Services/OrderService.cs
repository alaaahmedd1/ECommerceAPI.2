using ECommerce.Application.Common.Exceptions;
using ECommerce.Application.DTOs.Orders;
using ECommerce.Application.Interfaces.Repositories;
using ECommerce.Application.Interfaces.Services;
using ECommerce.Application.Mappings;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Enums;
using ECommerce.Domain.Exceptions;

namespace ECommerce.Application.Services;

public sealed class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IPaymentGateway _paymentGateway;
    private readonly IUnitOfWork _unitOfWork;

    public OrderService(
        IOrderRepository orderRepository,
        IProductRepository productRepository,
        ICustomerRepository customerRepository,
        IPaymentGateway paymentGateway,
        IUnitOfWork unitOfWork)
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
        _customerRepository = customerRepository;
        _paymentGateway = paymentGateway;
        _unitOfWork = unitOfWork;
    }

    public async Task<OrderResponse> GetOrderByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var order = await _orderRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException(nameof(Order), id);

        return order.ToResponse();
    }

    public async Task<IReadOnlyList<OrderResponse>> GetCustomerOrdersAsync(int customerId, CancellationToken cancellationToken = default)
    {
        var orders = await _orderRepository.GetByCustomerIdAsync(customerId, cancellationToken);
        return orders.Select(o => o.ToResponse()).ToList().AsReadOnly();
    }

    public async Task<CheckoutOrderResponse> CheckoutAsync(CreateOrderRequest request, CancellationToken cancellationToken = default)
    {
        if (request.Items == null || !request.Items.Any())
            throw new ValidationException("Cannot checkout an empty order.");

        var customer = await _customerRepository.GetByIdAsync(request.CustomerId, cancellationToken)
            ?? throw new NotFoundException(nameof(Customer), request.CustomerId);

        var productIds = request.Items.Select(i => i.ProductId).Distinct().ToList();
        var products = (await _productRepository.GetByIdsAsync(productIds, cancellationToken))
            .ToDictionary(p => p.Id);

        var order = new Order(customer.Id);

        foreach (var item in request.Items)
        {
            if (!products.TryGetValue(item.ProductId, out var product))
                throw new NotFoundException(nameof(Product), item.ProductId);

            product.DeductStock(item.Quantity);
            order.AddItem(product.Id, item.Quantity, product.Price);
        }

        Coupon? coupon = null;
        if (!string.IsNullOrWhiteSpace(request.CouponCode))
        {
            coupon = await _orderRepository.GetCouponByCodeAsync(request.CouponCode, cancellationToken)
                ?? throw new ValidationException($"Coupon '{request.CouponCode}' is invalid or does not exist.");

            if (!coupon.IsActive)
                throw new ValidationException($"Coupon '{request.CouponCode}' is expired or inactive.");
        }

        order.CalculateTotals(customer.IsVip, coupon);

        var paymentResult = await _paymentGateway.ChargeAsync(customer.Email, order.TotalAmount, cancellationToken);
        if (!paymentResult.IsSuccess)
            throw new DomainException($"Payment processing failed: {paymentResult.ErrorMessage ?? "Declined."}");

        order.MarkAsPaid(paymentResult.TransactionReference);

        await _orderRepository.AddAsync(order, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new CheckoutOrderResponse(
            order.Id,
            order.Status.ToString(),
            order.Subtotal,
            order.DiscountAmount,
            order.TaxAmount,
            order.ShippingFee,
            order.TotalAmount,
            paymentResult.TransactionReference
        );
    }

    public async Task CancelOrderAsync(int orderId, CancellationToken cancellationToken = default)
    {
        var order = await _orderRepository.GetByIdAsync(orderId, cancellationToken)
            ?? throw new NotFoundException(nameof(Order), orderId);

        var wasPaid = order.Status == OrderStatus.Paid;
        order.Cancel();

        if (wasPaid)
        {
            var productIds = order.Items.Select(i => i.ProductId).Distinct().ToList();
            var products = (await _productRepository.GetByIdsAsync(productIds, cancellationToken))
                .ToDictionary(p => p.Id);

            foreach (var item in order.Items)
            {
                if (products.TryGetValue(item.ProductId, out var product))
                    product.Restock(item.Quantity);
            }
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
