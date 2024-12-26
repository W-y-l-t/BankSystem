using BankSystem.Application.Application.Abstractions.DataAccess;
using BankSystem.Application.Application.Mapping;
using BankSystem.Domain.Domain.Transactions;
using MediatR;
using static BankSystem.Application.Application.Contracts.Transactions.GetAllTransactions;

namespace BankSystem.Application.Application.Transactions;

public class GetAllTransactionsHandler : IRequestHandler<Command, Response>
{
    private readonly IDataBaseContext _context;

    public GetAllTransactionsHandler(IDataBaseContext context)
    {
        _context = context;
    }

    public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
    {
        IReadOnlyCollection<Transaction> transactions = await _context.Transactions.GetByAccountId(request.AccountId);

        return new Response(transactions.Select(x => x.AsDto()).ToArray());
    }
}