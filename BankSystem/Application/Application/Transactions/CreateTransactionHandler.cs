using BankSystem.Application.Application.Abstractions.DataAccess;
using BankSystem.Application.Application.Mapping;
using BankSystem.Domain.Domain.Transactions;
using MediatR;
using static BankSystem.Application.Application.Contracts.Transactions.CreateTransaction;

namespace BankSystem.Application.Application.Transactions;

public class CreateTransactionHandler : IRequestHandler<Command, Response>
{
    private readonly IDataBaseContext _context;

    public CreateTransactionHandler(IDataBaseContext context)
    {
        _context = context;
    }

    public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
    {
        var transaction = new Transaction(Guid.NewGuid(), request.Amount, request.Type, request.AccountId);

        await _context.Transactions.Add(transaction);

        return new Response(transaction.AsDto());
    }
}