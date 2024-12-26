using Itmo.Dev.Platform.Common.Extensions;
using Itmo.Dev.Platform.Postgres.Extensions;
using Itmo.Dev.Platform.Postgres.Models;
using Itmo.Dev.Platform.Postgres.Plugins;
using BankSystem.Application.Application.Abstractions.DataAccess;
using BankSystem.Application.Application.Abstractions.Repositories;
using BankSystem.Infrastructure.Infrastructure.DataAccess.DataAccess;
using BankSystem.Infrastructure.Infrastructure.DataAccess.Plugins;
using BankSystem.Infrastructure.Infrastructure.DataAccess.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace BankSystem.Infrastructure.Infrastructure.DataAccess.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureDataAccess(
        this IServiceCollection collection,
        Action<PostgresConnectionConfiguration> configuration)
    {
        collection.AddPlatform();
        collection.AddPlatformPostgres(builder => builder.Configure(configuration));
        collection.AddPlatformMigrations(typeof(ServiceCollectionExtensions).Assembly);

        collection.AddSingleton<IDataSourcePlugin, MappingPlugin>();

        collection.AddScoped<IAccountRepository, AccountRepository>();
        collection.AddScoped<ISystemPasswordRepository, SystemPasswordRepository>();
        collection.AddScoped<ITransactionRepository, TransactionRepository>();
        collection.AddScoped<IUserRepository, UserRepository>();
        collection.AddScoped<IDataBaseContext, DataBaseContext>();

        return collection;
    }
}