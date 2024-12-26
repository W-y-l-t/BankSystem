namespace BankSystem.Domain.Domain.Common.ValueObjects;

public class PinCode : IEquatable<PinCode>
{
    public PinCode(string value)
    {
        if (!value.All(char.IsDigit) || value.Length != 4)
            throw new ArgumentException("Pin code must consist of 4 digits");

        Value = value;
    }

    public string Value { get; }

    public bool Equals(PinCode? other)
    {
        return other is not null
               && (ReferenceEquals(this, other) || Value == other.Value);
    }

    public override bool Equals(object? obj)
    {
        return obj is not null
               && (ReferenceEquals(this, obj)
                   || (obj.GetType() == GetType() && Equals((PinCode)obj)));
    }

    public override int GetHashCode()
    {
        return string.GetHashCode(Value, StringComparison.Ordinal);
    }
}