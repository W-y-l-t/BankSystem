using Itmo.Dev.Platform.Postgres.Connection;
using Itmo.Dev.Platform.Postgres.Extensions;
using BankSystem.Application.Application.Abstractions.Repositories;
using BankSystem.Domain.Domain.Users;
using Npgsql;

namespace BankSystem.Infrastructure.Infrastructure.DataAccess.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IPostgresConnectionProvider _connectionProvider;

    public UserRepository(IPostgresConnectionProvider connectionProvider)
    {
        _connectionProvider = connectionProvider;
    }

    public async Task<User?> FindById(Guid userId)
    {
        const string userSql = $"""
                                select user_id, user_name, user_role, user_password
                                from users
                                where user_id = :userId;
                                """;
        const string accountSql = $"""
                                   select account_id
                                   from accounts
                                   where account_owner_id = :userId;
                                   """;

        await using NpgsqlConnection userConnection =
            await _connectionProvider.GetConnectionAsync(default);
        await using NpgsqlConnection accountConnection =
            await _connectionProvider.GetConnectionAsync(default);

        await using NpgsqlCommand userCommand = new NpgsqlCommand(userSql, userConnection)
            .AddParameter("userId", userId);

        await using NpgsqlDataReader userReader = await userCommand.ExecuteReaderAsync();

        if (await userReader.ReadAsync() is false)
            return null;

        await using NpgsqlCommand accountsCommand = new NpgsqlCommand(userSql, accountConnection)
            .AddParameter("userId", userId);

        await using NpgsqlDataReader accountReader = await accountsCommand.ExecuteReaderAsync();

        List<Guid> accounts = [];

        while (await accountReader.ReadAsync())
        {
            accounts.Add(accountReader.GetGuid(0));
        }

        return new User(
            id: userReader.GetGuid(0),
            name: userReader.GetString(1),
            role: userReader.GetFieldValue<UserRole>(2),
            password: userReader.GetString(3),
            accounts: accounts);
    }

    public async Task<User?> FindByAccountProperties(string name, string password)
    {
        const string userSql = $"""
                                select user_id, user_name, user_role, user_password
                                from users
                                where user_name = :userName and user_password = :userPassword;
                                """;
        const string accountSql = $"""
                                   select account_id
                                   from accounts
                                   where account_owner_id = :userId;
                                   """;

        await using NpgsqlConnection userConnection =
            await _connectionProvider.GetConnectionAsync(default);
        await using NpgsqlConnection accountConnection =
            await _connectionProvider.GetConnectionAsync(default);

        await using NpgsqlCommand userCommand = new NpgsqlCommand(userSql, userConnection)
            .AddParameter("userName", name)
            .AddParameter("userPassword", password);

        await using NpgsqlDataReader userReader = await userCommand.ExecuteReaderAsync();

        if (await userReader.ReadAsync() is false)
            return null;

        Guid userId = userReader.GetGuid(0);

        await using NpgsqlCommand accountsCommand = new NpgsqlCommand(accountSql, accountConnection)
            .AddParameter("userId", userId);

        await using NpgsqlDataReader accountReader = await accountsCommand.ExecuteReaderAsync();

        List<Guid> accounts = [];

        while (await accountReader.ReadAsync())
        {
            accounts.Add(accountReader.GetGuid(0));
        }

        return new User(
            id: userId,
            name: userReader.GetString(1),
            role: userReader.GetFieldValue<UserRole>(2),
            password: userReader.GetString(3),
            accounts: accounts);
    }

    public async Task Add(User user)
    {
        const string sql = $"""
                            insert into users (user_id, user_name, user_role, user_password)
                            values (:userId, :userName, :userRole, :userPassword);
                            """;

        await using NpgsqlConnection connection = await _connectionProvider.GetConnectionAsync(default);

        await using NpgsqlCommand command = new NpgsqlCommand(sql, connection)
            .AddParameter("userId", user.Id)
            .AddParameter("userName", user.Name)
            .AddParameter("userRole", user.Role)
            .AddParameter("userPassword", user.Password);

        int rowsAffected = await command.ExecuteNonQueryAsync();

        if (rowsAffected != 1)
            throw new ArgumentException("Failed to insert user");
    }

    public async Task Update(User user)
    {
        const string sql = $"""
                            update users
                            set user_name = :userName,
                                user_role = :userRole,
                                user_password = :userPassword
                            where user_id = :userId;
                            """;

        await using NpgsqlConnection connection = await _connectionProvider.GetConnectionAsync(default);

        await using NpgsqlCommand command = new NpgsqlCommand(sql, connection)
            .AddParameter("userId", user.Id)
            .AddParameter("userName", user.Name)
            .AddParameter("userRole", user.Role)
            .AddParameter("userPassword", user.Password);

        int rowsAffected = await command.ExecuteNonQueryAsync();

        if (rowsAffected != 1)
            throw new ArgumentException($"Failed to update user with ID {user.Id}");
    }
}