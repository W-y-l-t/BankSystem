using Itmo.Dev.Platform.Postgres.Models;
using BankSystem.Application.Application.Extensions;
using BankSystem.Infrastructure.Infrastructure.DataAccess.Extensions;
using BankSystem.Presentation.Extensions;
using BankSystem.Presentation.Scenarios;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console;
using System.Data;

namespace BankSystem;

public static class Program
{
    public static async Task Main(string[] args)
    {
        IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        PostgresConnectionConfiguration postgresConfig = configuration
            .GetSection("DatabaseConfiguration")
            .Get<PostgresConnectionConfiguration>() ?? throw new DataException("DatabaseConfiguration is missing");

        var collection = new ServiceCollection();

        collection
            .AddApplication()
            .AddInfrastructureDataAccess(cfg =>
            {
                cfg.Host = postgresConfig.Host;
                cfg.Port = postgresConfig.Port;
                cfg.Username = postgresConfig.Username;
                cfg.Password = postgresConfig.Password;
                cfg.Database = postgresConfig.Database;
                cfg.SslMode = postgresConfig.SslMode;
            })
            .AddPresentationConsole();

        ServiceProvider provider = collection.BuildServiceProvider();
        using IServiceScope scope = provider.CreateScope();

        await scope.UseInfrastructureDataAccess();

        ScenarioRunner scenarioRunner = scope.ServiceProvider.GetRequiredService<ScenarioRunner>();

        while (true)
        {
            await scenarioRunner.Run();
            AnsiConsole.Clear();
        }
    }
}