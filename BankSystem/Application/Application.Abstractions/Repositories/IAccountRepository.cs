using BankSystem.Domain.Domain.Accounts;

namespace BankSystem.Application.Application.Abstractions.Repositories;

public interface IAccountRepository
{
    Task<Account?> FindById(Guid accountId);

    Task Add(Account account);

    Task Update(Account account);

    Task<IReadOnlyCollection<Guid>> GetAll();
}