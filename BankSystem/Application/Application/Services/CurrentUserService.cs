using BankSystem.Application.Application.Abstractions.Services;
using BankSystem.Domain.Domain.Users;

namespace BankSystem.Application.Application.Services;

public class CurrentUserService : ICurrentUserService
{
    public User? CurrentUser { get; private set; }

    public Task SetCurrentUser(User user)
    {
        CurrentUser = user;

        return Task.CompletedTask;
    }

    public Task ClearContext()
    {
        CurrentUser = null;

        return Task.CompletedTask;
    }
}