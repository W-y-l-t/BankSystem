using BankSystem.Application.Application.Dto;
using MediatR;

namespace BankSystem.Application.Application.Contracts.Accounts;

public static class ShowHistory
{
    public record struct Command(Guid Id) : IRequest<Response>;

    public record struct Response(IReadOnlyCollection<TransactionDto> Transactions);
}