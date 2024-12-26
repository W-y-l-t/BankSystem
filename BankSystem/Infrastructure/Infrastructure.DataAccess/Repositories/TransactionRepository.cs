using Itmo.Dev.Platform.Postgres.Connection;
using Itmo.Dev.Platform.Postgres.Extensions;
using BankSystem.Application.Application.Abstractions.Repositories;
using BankSystem.Domain.Domain.Common.ValueObjects;
using BankSystem.Domain.Domain.Transactions;
using Npgsql;

namespace BankSystem.Infrastructure.Infrastructure.DataAccess.Repositories;

public class TransactionRepository : ITransactionRepository
{
    private readonly IPostgresConnectionProvider _connectionProvider;

    public TransactionRepository(IPostgresConnectionProvider connectionProvider)
    {
        _connectionProvider = connectionProvider;
    }

    public async Task<Transaction> GetById(Guid transactionId)
    {
        const string transactionSql = $"""
                                select transaction_id, transaction_type, transaction_amount,
                                       transaction_account_id, transaction_datetime, transaction_status
                                from transactions
                                where transaction_id = :transactionId;
                                """;

        await using NpgsqlConnection connection = await _connectionProvider.GetConnectionAsync(default);

        await using NpgsqlCommand transactionCommand = new NpgsqlCommand(transactionSql, connection)
            .AddParameter("transactionId", transactionId);

        await using NpgsqlDataReader transactionReader = await transactionCommand.ExecuteReaderAsync();

        if (await transactionReader.ReadAsync() is false)
            throw new ArgumentException($"Transaction with ID {transactionId} not found");

        return new Transaction(
            id: transactionReader.GetGuid(0),
            transactionType: transactionReader.GetFieldValue<TransactionType>(1),
            amount: new Money(transactionReader.GetDecimal(2)),
            accountId: transactionReader.GetGuid(3),
            createdAt: DateTime.Parse(transactionReader.GetString(4)),
            status: transactionReader.GetFieldValue<TransactionStatus>(5));
    }

    public async Task Add(Transaction transaction)
    {
        const string addSql = $"""
                        insert into transactions (transaction_id, transaction_type, transaction_amount,
                                                  transaction_account_id, transaction_datetime, transaction_status)
                        values (:transactionId, :transactionType, :transactionAmount,
                                :accountId, :createdAt, :transactionStatus);
                        """;

        await using NpgsqlConnection connection = await _connectionProvider.GetConnectionAsync(default);

        await using NpgsqlCommand transactionCommand = new NpgsqlCommand(addSql, connection)
            .AddParameter("transactionId", transaction.Id)
            .AddParameter("transactionType", transaction.Type)
            .AddParameter("transactionAmount", transaction.Amount.Value)
            .AddParameter("accountId", transaction.AccountId)
            .AddParameter("createdAt", transaction.CreatedAt)
            .AddParameter("transactionStatus", transaction.Status);

        int rowsAffected = await transactionCommand.ExecuteNonQueryAsync();

        if (rowsAffected != 1)
            throw new ArgumentException("Failed to insert transaction");
    }

    public async Task Update(Transaction transaction)
    {
        const string updateSql = $"""
                                      update transactions
                                      set transaction_type = :transactionType,
                                          transaction_amount = :transactionAmount,
                                          transaction_account_id = :accountId,
                                          transaction_datetime = :createdAt,
                                          transaction_status = :transactionStatus
                                      where transaction_id = :transactionId;
                                  """;

        await using NpgsqlConnection connection = await _connectionProvider.GetConnectionAsync(default);

        await using NpgsqlCommand transactionCommand = new NpgsqlCommand(updateSql, connection)
            .AddParameter("transactionId", transaction.Id)
            .AddParameter("transactionType", transaction.Type)
            .AddParameter("transactionAmount", transaction.Amount.Value)
            .AddParameter("accountId", transaction.AccountId)
            .AddParameter("createdAt", transaction.CreatedAt)
            .AddParameter("transactionStatus", transaction.Status);

        int rowsAffected = await transactionCommand.ExecuteNonQueryAsync();

        if (rowsAffected != 1)
            throw new Exception($"Failed to update transaction with ID {transaction.Id}");
    }

    public async Task<IReadOnlyCollection<Transaction>> GetByAccountId(Guid accountId)
    {
        const string transactionSql = $"""
                                       select transaction_id, transaction_type, transaction_amount,
                                              transaction_account_id, transaction_datetime, transaction_status
                                       from transactions
                                       where transaction_account_id = :accountId;
                                       """;

        await using NpgsqlConnection connection = await _connectionProvider.GetConnectionAsync(default);

        await using NpgsqlCommand transactionCommand = new NpgsqlCommand(transactionSql, connection)
            .AddParameter("accountId", accountId);

        await using NpgsqlDataReader transactionReader = await transactionCommand.ExecuteReaderAsync();

        List<Transaction> transactions = [];

        while (await transactionReader.ReadAsync())
        {
            var transaction = new Transaction(
                id: transactionReader.GetGuid(0),
                transactionType: transactionReader.GetFieldValue<TransactionType>(1),
                amount: new Money(transactionReader.GetDecimal(2)),
                accountId: transactionReader.GetGuid(3),
                createdAt: DateTime.Parse(transactionReader.GetString(4)),
                status: transactionReader.GetFieldValue<TransactionStatus>(5));

            transactions.Add(transaction);
        }

        return transactions;
    }
}