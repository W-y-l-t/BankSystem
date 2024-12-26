using Itmo.Dev.Platform.Postgres.Plugins;
using BankSystem.Domain.Domain.Transactions;
using BankSystem.Domain.Domain.Users;
using Npgsql;

namespace BankSystem.Infrastructure.Infrastructure.DataAccess.Plugins;

public class MappingPlugin : IDataSourcePlugin
{
    public void Configure(NpgsqlDataSourceBuilder builder)
    {
        builder.MapEnum<TransactionStatus>();
        builder.MapEnum<TransactionType>();
        builder.MapEnum<UserRole>();
    }
}