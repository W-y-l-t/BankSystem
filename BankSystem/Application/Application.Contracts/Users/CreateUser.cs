using BankSystem.Application.Application.Dto;
using BankSystem.Domain.Domain.Users;
using MediatR;

namespace BankSystem.Application.Application.Contracts.Users;

public static class CreateUser
{
    public record struct Command(string Name, UserRole Role, string Password) : IRequest<Response>;

    public record struct Response(UserDto User);
}