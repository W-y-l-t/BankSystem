using BankSystem.Application.Application.Abstractions.Services;
using BankSystem.Domain.Domain.Accounts;

namespace BankSystem.Application.Application.Services;

public class CurrentAccountService : ICurrentAccountService
{
    public Account? CurrentAccount { get; private set; }

    public Task SetCurrentAccount(Account account)
    {
        CurrentAccount = account;

        return Task.CompletedTask;
    }

    public Task ClearContext()
    {
        CurrentAccount = null;

        return Task.CompletedTask;
    }
}