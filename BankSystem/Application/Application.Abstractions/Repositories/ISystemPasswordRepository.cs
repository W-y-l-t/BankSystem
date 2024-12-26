namespace BankSystem.Application.Application.Abstractions.Repositories;

public interface ISystemPasswordRepository
{
    Task<string?> FindPassword(string password);

    Task Add(string password);
}