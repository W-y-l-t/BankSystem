using BankSystem.Application.Application.Abstractions.DataAccess;
using BankSystem.Application.Application.Mapping;
using BankSystem.Domain.Domain.Transactions;
using MediatR;
using static BankSystem.Application.Application.Contracts.Accounts.ShowHistory;

namespace BankSystem.Application.Application.Accounts;

public class ShowHistoryHandler : IRequestHandler<Command, Response>
{
    private readonly IDataBaseContext _context;

    public ShowHistoryHandler(IDataBaseContext context)
    {
        _context = context;
    }

    public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
    {
        IReadOnlyCollection<Transaction> transactions = await _context.Transactions.GetByAccountId(request.Id);

        return new Response(transactions.Select(x => x.AsDto()).ToArray());
    }
}