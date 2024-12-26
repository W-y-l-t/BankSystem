using Itmo.Dev.Platform.Postgres.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace BankSystem.Infrastructure.Infrastructure.DataAccess.Extensions;

public static class ServiceScopeExtensions
{
    public static async Task UseInfrastructureDataAccess(this IServiceScope scope)
    {
        await scope.UsePlatformMigrationsAsync(default);
    }
}