using BankSystem.Application.Application.Abstractions.Repositories;

namespace BankSystem.Application.Application.Abstractions.DataAccess;

public interface IDataBaseContext
{
    IAccountRepository Accounts { get; }

    ITransactionRepository Transactions { get; }

    IUserRepository Users { get; }

    ISystemPasswordRepository SystemPasswords { get; }
}