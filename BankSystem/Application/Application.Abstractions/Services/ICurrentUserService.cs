using BankSystem.Domain.Domain.Users;

namespace BankSystem.Application.Application.Abstractions.Services;

public interface ICurrentUserService
{
    User? CurrentUser { get; }

    Task SetCurrentUser(User user);

    Task ClearContext();
}