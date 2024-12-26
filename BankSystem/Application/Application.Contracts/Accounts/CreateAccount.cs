using BankSystem.Application.Application.Dto;
using BankSystem.Domain.Domain.Common.ValueObjects;
using MediatR;

namespace BankSystem.Application.Application.Contracts.Accounts;

public static class CreateAccount
{
    public record struct Command(PinCode PinCode, Guid OwnerId) : IRequest<Response>;

    public record struct Response(AccountDto Account);
}