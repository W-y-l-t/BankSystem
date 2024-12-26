using BankSystem.Application.Application.Abstractions.DataAccess;
using MediatR;
using static BankSystem.Application.Application.Contracts.Accounts.GetAllAccounts;

namespace BankSystem.Application.Application.Accounts;

public class GetAllAccountsHandler : IRequestHandler<Command, Response>
{
    private readonly IDataBaseContext _context;

    public GetAllAccountsHandler(IDataBaseContext context)
    {
        _context = context;
    }

    public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
    {
        IReadOnlyCollection<Guid> accounts = await _context.Accounts.GetAll();

        return new Response(accounts);
    }
}