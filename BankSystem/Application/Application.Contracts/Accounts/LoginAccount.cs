using BankSystem.Application.Application.Dto;
using BankSystem.Domain.Domain.Common.ValueObjects;
using MediatR;

namespace BankSystem.Application.Application.Contracts.Accounts;

public static class LoginAccount
{
    public record struct Command(Guid Id, PinCode PinCode) : IRequest<Response>;

    public record struct Response(AccountDto? Account, bool IsAuthenticated);
}