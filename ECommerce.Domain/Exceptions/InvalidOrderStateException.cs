namespace ECommerce.Domain.Exceptions;

public sealed class InvalidOrderStateException : DomainException
{
    public InvalidOrderStateException(string message) : base(message) { }
}
