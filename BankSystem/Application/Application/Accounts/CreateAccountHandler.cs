using BankSystem.Application.Application.Abstractions.DataAccess;
using BankSystem.Application.Application.Abstractions.Services;
using BankSystem.Application.Application.Mapping;
using BankSystem.Domain.Domain.Accounts;
using MediatR;
using static BankSystem.Application.Application.Contracts.Accounts.CreateAccount;

namespace BankSystem.Application.Application.Accounts;

public class CreateAccountHandler : IRequestHandler<Command, Response>
{
    private readonly IDataBaseContext _context;
    private readonly ICurrentUserService _currentUserService;

    public CreateAccountHandler(IDataBaseContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
    {
        if (_currentUserService.CurrentUser is null)
            throw new ArgumentException("You are not authorized to access this resource.");

        var account = new Account(Guid.NewGuid(), request.PinCode, _currentUserService.CurrentUser.Id);

        _currentUserService.CurrentUser.AddAccount(account.Id);

        await _context.Accounts.Add(account);

        return new Response(account.AsDto());
    }
}