using BankSystem.Application.Application.Dto;
using MediatR;

namespace BankSystem.Application.Application.Contracts.Transactions;

public static class GetTransaction
{
    public record struct Command(Guid TransactionId) : IRequest<Response>;

    public record struct Response(TransactionDto Transaction);
}