using BankSystem.Application.Application.Abstractions.DataAccess;
using BankSystem.Application.Application.Mapping;
using MediatR;
using static BankSystem.Application.Application.Contracts.Transactions.GetTransaction;
using Transaction = BankSystem.Domain.Domain.Transactions.Transaction;

namespace BankSystem.Application.Application.Transactions;

public class GetTransactionHandler : IRequestHandler<Command, Response>
{
    private readonly IDataBaseContext _context;

    public GetTransactionHandler(IDataBaseContext context)
    {
        _context = context;
    }

    public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
    {
        Transaction transaction = await _context.Transactions.GetById(request.TransactionId);

        return new Response(transaction.AsDto());
    }
}