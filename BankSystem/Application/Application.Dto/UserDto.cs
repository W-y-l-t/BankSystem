using BankSystem.Domain.Domain.Users;

namespace BankSystem.Application.Application.Dto;

public record UserDto(Guid Id, UserRole Role, string Name, string Password, IReadOnlyCollection<Guid> Accounts);