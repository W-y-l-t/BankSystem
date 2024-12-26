namespace BankSystem.Domain.Domain.Common.ValueObjects;

public class Money
{
    public Money(decimal value)
    {
        if (value < 0)
            throw new ArgumentException("The amount of money should not be negative.");

        Value = value;
    }

    public decimal Value { get; set; }

    public static bool operator <(Money left, Money right)
    {
        return left.Value < right.Value;
    }

    public static bool operator >(Money left, Money right)
    {
        return left.Value > right.Value;
    }
}