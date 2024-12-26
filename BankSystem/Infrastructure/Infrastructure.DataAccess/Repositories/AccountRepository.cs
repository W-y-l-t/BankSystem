using Itmo.Dev.Platform.Postgres.Connection;
using Itmo.Dev.Platform.Postgres.Extensions;
using BankSystem.Application.Application.Abstractions.Repositories;
using BankSystem.Domain.Domain.Accounts;
using BankSystem.Domain.Domain.Common.ValueObjects;
using Npgsql;

namespace BankSystem.Infrastructure.Infrastructure.DataAccess.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly IPostgresConnectionProvider _connectionProvider;

    public AccountRepository(IPostgresConnectionProvider connectionProvider)
    {
        _connectionProvider = connectionProvider;
    }

    public async Task<Account?> FindById(Guid accountId)
    {
        const string accountSql = $"""
                            select account_id, account_pin_code, account_balance, account_owner_id
                            from accounts
                            where account_id = :accountId;
                            """;
        const string transactionSql = $"""
                                       select transaction_id
                                       from transactions
                                       where account_id = :accountId;
                                       """;

        await using NpgsqlConnection accountConnection =
            await _connectionProvider.GetConnectionAsync(default);
        await using NpgsqlConnection transactionConnection =
            await _connectionProvider.GetConnectionAsync(default);

        await using NpgsqlCommand accountCommand = new NpgsqlCommand(accountSql, accountConnection)
            .AddParameter("accountId", accountId);

        await using NpgsqlDataReader accountReader = await accountCommand.ExecuteReaderAsync();

        if (await accountReader.ReadAsync() is false)
            return null;

        await using NpgsqlCommand transactionCommand = new NpgsqlCommand(accountSql, transactionConnection)
            .AddParameter("accountId", accountId);

        await using NpgsqlDataReader transactionReader = await transactionCommand.ExecuteReaderAsync();

        List<Guid> history = [];

        while (await transactionReader.ReadAsync())
        {
            history.Add(transactionReader.GetGuid(0));
        }

        return new Account(
            id: accountReader.GetGuid(0),
            pinCode: new PinCode(accountReader.GetString(1)),
            balance: new Money(accountReader.GetDecimal(2)),
            ownerId: accountReader.GetGuid(3),
            history: history);
    }

    public async Task Add(Account account)
    {
        const string sql = $"""
                            insert into accounts (account_id, account_pin_code, account_balance, account_owner_id)
                            values (:accountId, :accountPinCode, :accountBalance, :accountOwnerId);
                            """;

        await using NpgsqlConnection connection = await _connectionProvider.GetConnectionAsync(default);

        await using NpgsqlCommand command = new NpgsqlCommand(sql, connection)
            .AddParameter("accountId", account.Id)
            .AddParameter("accountPinCode", account.PinCode.Value)
            .AddParameter("accountBalance", account.Balance.Value)
            .AddParameter("accountOwnerId", account.OwnerId);

        int rowsAffected = await command.ExecuteNonQueryAsync();

        if (rowsAffected != 1)
            throw new ArgumentException("Failed to insert account");
    }

    public async Task Update(Account account)
    {
        const string sql = $"""
                            update accounts
                            set account_pin_code = :accountPinCode,
                                account_balance = :accountBalance,
                                account_owner_id = :accountOwnerId
                            where account_id = :accountId;
                            """;

        await using NpgsqlConnection connection = await _connectionProvider.GetConnectionAsync(default);

        await using NpgsqlCommand command = new NpgsqlCommand(sql, connection)
            .AddParameter("accountId", account.Id)
            .AddParameter("accountPinCode", account.PinCode.Value)
            .AddParameter("accountBalance", account.Balance.Value)
            .AddParameter("accountOwnerId", account.OwnerId);

        int rowsAffected = await command.ExecuteNonQueryAsync();

        if (rowsAffected != 1)
            throw new ArgumentException($"Failed to account user with ID {account.Id}");
    }

    public async Task<IReadOnlyCollection<Guid>> GetAll()
    {
        const string sql = $"""
                            select account_id
                            from accounts
                            """;

        await using NpgsqlConnection connection =
            await _connectionProvider.GetConnectionAsync(default);

        await using var command = new NpgsqlCommand(sql, connection);

        await using NpgsqlDataReader reader = await command.ExecuteReaderAsync();

        List<Guid> accounts = [];

        while (await reader.ReadAsync())
        {
            accounts.Add(reader.GetGuid(0));
        }

        return accounts.AsReadOnly();
    }
}