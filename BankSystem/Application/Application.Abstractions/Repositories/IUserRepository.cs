using BankSystem.Domain.Domain.Users;

namespace BankSystem.Application.Application.Abstractions.Repositories;

public interface IUserRepository
{
    Task<User?> FindById(Guid userId);

    Task<User?> FindByAccountProperties(string name, string password);

    Task Add(User user);

    Task Update(User user);
}