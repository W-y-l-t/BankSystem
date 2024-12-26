using BankSystem.Domain.Domain.Accounts;

namespace BankSystem.Application.Application.Abstractions.Services;

public interface ICurrentAccountService
{
    Account? CurrentAccount { get; }

    Task SetCurrentAccount(Account account);

    Task ClearContext();
}