using BankSystem.Domain.Domain.Accounts;
using BankSystem.Domain.Domain.Common.ValueObjects;

namespace BankSystem.Domain.Domain.Transactions;

public class Transaction
{
    public Transaction(Guid id, Money amount, TransactionType transactionType, Guid accountId)
    {
        Id = id;
        Amount = amount;
        Type = transactionType;
        AccountId = accountId;
        CreatedAt = DateTime.UtcNow;
        Status = TransactionStatus.Created;
    }

    public Transaction(
        Guid id,
        Money amount,
        TransactionType transactionType,
        Guid accountId,
        DateTime createdAt,
        TransactionStatus status)
        : this(id, amount, transactionType, accountId)
    {
        CreatedAt = createdAt;
        Status = status;
    }

    public Guid Id { get; }

    public Money Amount { get; }

    public TransactionType Type { get; }

    public Guid AccountId { get; }

    public DateTime CreatedAt { get; }

    public TransactionStatus Status { get; private set; }

    public void Enforce(Account account)
    {
        if (Status != TransactionStatus.Created)
        {
            Status = TransactionStatus.Undefined;

            return;
        }

        if (account.Id != AccountId)
        {
            Status = TransactionStatus.Rejected;

            return;
        }

        if (Type is TransactionType.Deposit)
        {
            account.ApplyTransaction(this, (a, b) => b.Value += a.Value);
        }
        else
        {
            if (account.Balance < Amount)
            {
                Status = TransactionStatus.Rejected;
                return;
            }

            account.ApplyTransaction(this, (a, b) => b.Value -= a.Value);
        }

        Status = TransactionStatus.Completed;
    }
}