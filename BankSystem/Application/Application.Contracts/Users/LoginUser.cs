using BankSystem.Application.Application.Dto;
using MediatR;

namespace BankSystem.Application.Application.Contracts.Users;

public static class LoginUser
{
    public record struct Command(string Name, string Password) : IRequest<Response>;

    public record struct Response(UserDto? User, bool IsAuthenticated);
}