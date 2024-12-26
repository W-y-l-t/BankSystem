using BankSystem.Domain.Domain.Common.ValueObjects;
using BankSystem.Domain.Domain.Transactions;

namespace BankSystem.Domain.Domain.Accounts;

public class Account
{
    private readonly List<Guid> _history;

    public Account(Guid id, PinCode pinCode, Guid ownerId)
    {
        Id = id;
        PinCode = pinCode;
        OwnerId = ownerId;
        Balance = new Money(0);
        _history = [];
    }

    public Account(Guid id, PinCode pinCode, Guid ownerId, Money balance, IReadOnlyCollection<Guid> history)
        : this(id, pinCode, ownerId)
    {
        Balance = balance;
        _history = [.. history];
    }

    public Guid Id { get; }

    public Money Balance { get; }

    public PinCode PinCode { get; }

    public IReadOnlyCollection<Guid> History => _history;

    public Guid OwnerId { get; }

    public void ApplyTransaction(Transaction transaction, Action<Money, Money> applyAction)
    {
        applyAction(transaction.Amount, Balance);

        _history.Add(transaction.Id);
    }
}