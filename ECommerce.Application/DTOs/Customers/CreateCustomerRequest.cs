namespace ECommerce.Application.DTOs.Customers;

public record CreateCustomerRequest(string FullName, string Email, bool IsVip);
