using BankSystem.Application.Application.Dto;
using BankSystem.Domain.Domain.Transactions;

namespace BankSystem.Application.Application.Mapping;

public static class TransactionMapping
{
    public static TransactionDto AsDto(this Transaction transaction)
    {
        return new TransactionDto(
            transaction.Id,
            transaction.Amount,
            transaction.Type,
            transaction.CreatedAt,
            transaction.AccountId,
            transaction.Status);
    }
}