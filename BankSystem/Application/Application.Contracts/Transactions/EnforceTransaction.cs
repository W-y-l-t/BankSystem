using MediatR;

namespace BankSystem.Application.Application.Contracts.Transactions;

public static class EnforceTransaction
{
    public record struct Command(Guid OrderId) : IRequest;
}