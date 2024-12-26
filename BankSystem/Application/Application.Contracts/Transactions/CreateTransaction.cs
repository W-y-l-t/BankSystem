using BankSystem.Application.Application.Dto;
using BankSystem.Domain.Domain.Common.ValueObjects;
using BankSystem.Domain.Domain.Transactions;
using MediatR;

namespace BankSystem.Application.Application.Contracts.Transactions;

public static class CreateTransaction
{
    public record struct Command(Money Amount, TransactionType Type, Guid AccountId) : IRequest<Response>;

    public record struct Response(TransactionDto Transaction);
}