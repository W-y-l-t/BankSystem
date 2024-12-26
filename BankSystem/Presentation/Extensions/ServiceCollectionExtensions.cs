using BankSystem.Presentation.ScenarioProviders;
using BankSystem.Presentation.ScenarioProviders.Accounts;
using BankSystem.Presentation.ScenarioProviders.System;
using BankSystem.Presentation.ScenarioProviders.Transactions;
using BankSystem.Presentation.ScenarioProviders.Users;
using BankSystem.Presentation.Scenarios;
using Microsoft.Extensions.DependencyInjection;

namespace BankSystem.Presentation.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPresentationConsole(this IServiceCollection collection)
    {
        collection.AddScoped<ScenarioRunner>();

        collection.AddScoped<IScenarioProvider, CreateUserScenarioProvider>();
        collection.AddScoped<IScenarioProvider, LoginUserScenarioProvider>();

        collection.AddScoped<IScenarioProvider, CreateAccountScenarioProvider>();
        collection.AddScoped<IScenarioProvider, LoginAccountScenarioProvider>();
        collection.AddScoped<IScenarioProvider, ShowHistoryScenarioProvider>();
        collection.AddScoped<IScenarioProvider, ShowBalanceScenarioProvider>();

        collection.AddScoped<IScenarioProvider, CreateTransactionScenarioProvider>();
        collection.AddScoped<IScenarioProvider, EnforceTransactionScenarioProvider>();
        collection.AddScoped<IScenarioProvider, AdminTransactionViewScenarioProvider>();

        collection.AddScoped<IScenarioProvider, ExitScenarioProvider>();
        collection.AddScoped<IScenarioProvider, ChangeUserScenarioProvider>();
        collection.AddScoped<IScenarioProvider, ChangeAccountScenarioProvider>();

        return collection;
    }
}