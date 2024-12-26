using BankSystem.Application.Application.Abstractions.Services;
using BankSystem.Application.Application.Services;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace BankSystem.Application.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection collection)
    {
        collection.AddMediatR(typeof(IAssemblyMaker));

        collection.AddScoped<ICurrentUserService, CurrentUserService>();
        collection.AddScoped<ICurrentAccountService, CurrentAccountService>();

        return collection;
    }
}