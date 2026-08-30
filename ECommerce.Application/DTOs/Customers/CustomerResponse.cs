namespace ECommerce.Application.DTOs.Customers;

public record CustomerResponse(int Id, string FullName, string Email, bool IsVip);
