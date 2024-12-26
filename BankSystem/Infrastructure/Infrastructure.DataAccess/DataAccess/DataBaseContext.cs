using BankSystem.Application.Application.Abstractions.DataAccess;
using BankSystem.Application.Application.Abstractions.Repositories;

namespace BankSystem.Infrastructure.Infrastructure.DataAccess.DataAccess;

public class DataBaseContext : IDataBaseContext
{
    public DataBaseContext(
        IAccountRepository accountRepository,
        ITransactionRepository transactionRepository,
        IUserRepository userRepository,
        ISystemPasswordRepository passwordRepository)
    {
        Accounts = accountRepository;
        Transactions = transactionRepository;
        Users = userRepository;
        SystemPasswords = passwordRepository;
    }

    public IAccountRepository Accounts { get; }

    public ITransactionRepository Transactions { get; }

    public IUserRepository Users { get; }

    public ISystemPasswordRepository SystemPasswords { get; }
}