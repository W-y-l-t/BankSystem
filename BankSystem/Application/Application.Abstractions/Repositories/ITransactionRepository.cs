using BankSystem.Domain.Domain.Transactions;

namespace BankSystem.Application.Application.Abstractions.Repositories;

public interface ITransactionRepository
{
    Task<Transaction> GetById(Guid transactionId);

    Task Add(Transaction transaction);

    Task Update(Transaction transaction);

    Task<IReadOnlyCollection<Transaction>> GetByAccountId(Guid accountId);
}