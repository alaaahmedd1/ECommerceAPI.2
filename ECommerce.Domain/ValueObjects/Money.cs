using ECommerce.Domain.Exceptions;

namespace ECommerce.Domain.ValueObjects;

public readonly record struct Money
{
    public decimal Amount { get; }

    public Money(decimal amount)
    {
        if (amount < 0)
            throw new DomainException("Money amount cannot be negative.");
        Amount = Math.Round(amount, 2);
    }

    public static Money Zero => new(0m);
    public static Money operator +(Money a, Money b) => new(a.Amount + b.Amount);
    public static Money operator -(Money a, Money b) => new(Math.Max(0m, a.Amount - b.Amount));
    public static Money operator *(Money a, decimal multiplier) => new(a.Amount * multiplier);
    public static implicit operator decimal(Money money) => money.Amount;
    public static explicit operator Money(decimal amount) => new(amount);
    public override string ToString() => Amount.ToString("C2");
}
