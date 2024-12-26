using Itmo.Dev.Platform.Postgres.Connection;
using Itmo.Dev.Platform.Postgres.Extensions;
using BankSystem.Application.Application.Abstractions.Repositories;
using BankSystem.Application.Application.Services;
using Npgsql;

namespace BankSystem.Infrastructure.Infrastructure.DataAccess.Repositories;

public class SystemPasswordRepository : ISystemPasswordRepository
{
    private readonly IPostgresConnectionProvider _connectionProvider;

    public SystemPasswordRepository(IPostgresConnectionProvider connectionProvider)
    {
        _connectionProvider = connectionProvider;
    }

    public async Task<string?> FindPassword(string password)
    {
        const string systemPasswordSql = $"""
                                          select system_password
                                          from system_passwords
                                          where system_password = :password;
                                          """;

        await using NpgsqlConnection connection = await _connectionProvider.GetConnectionAsync(default);

        await using NpgsqlCommand systemPasswordCommand = new NpgsqlCommand(systemPasswordSql, connection)
            .AddParameter("password", password);

        await using NpgsqlDataReader systemPasswordReader = await systemPasswordCommand.ExecuteReaderAsync();

        return await systemPasswordReader.ReadAsync() is false ? null : systemPasswordReader.GetString(0);
    }

    public async Task Add(string password)
    {
        string hash = PasswordHasherService.HashPassword(password);

        const string insertSystemPasswordSql = $"""
                                                insert into system_passwords (system_password)
                                                values (:hash);
                                                """;

        await using NpgsqlConnection connection = await _connectionProvider.GetConnectionAsync(default);

        await using NpgsqlCommand insertCommand = new NpgsqlCommand(insertSystemPasswordSql, connection)
            .AddParameter("hash", hash);

        int affectedRows = await insertCommand.ExecuteNonQueryAsync();

        if (affectedRows == 0)
            throw new InvalidOperationException("Failed to add system password.");
    }
}