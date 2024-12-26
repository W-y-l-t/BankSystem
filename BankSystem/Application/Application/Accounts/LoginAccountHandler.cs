using BankSystem.Application.Application.Abstractions.DataAccess;
using BankSystem.Application.Application.Mapping;
using BankSystem.Domain.Domain.Accounts;
using MediatR;
using static BankSystem.Application.Application.Contracts.Accounts.LoginAccount;

namespace BankSystem.Application.Application.Accounts;

public class LoginAccountHandler : IRequestHandler<Command, Response>
{
    private readonly IDataBaseContext _context;

    public LoginAccountHandler(IDataBaseContext context)
    {
        _context = context;
    }

    public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
    {
        Account? account = await _context.Accounts.FindById(request.Id);

        return account is not null && account.PinCode.Equals(request.PinCode)
            ? new Response(account.AsDto(), true)
            : new Response(null, false);
    }
}