using BankSystem.Application.Application.Dto;
using MediatR;

namespace BankSystem.Application.Application.Contracts.Transactions;

public static class GetAllTransactions
{
    public record struct Command(Guid AccountId) : IRequest<Response>;

    public record struct Response(IReadOnlyCollection<TransactionDto> Transactions);
}