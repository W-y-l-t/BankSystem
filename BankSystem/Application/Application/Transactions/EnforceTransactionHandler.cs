using BankSystem.Application.Application.Abstractions.DataAccess;
using BankSystem.Application.Application.Abstractions.Services;
using BankSystem.Domain.Domain.Accounts;
using BankSystem.Domain.Domain.Transactions;
using MediatR;
using static BankSystem.Application.Application.Contracts.Transactions.EnforceTransaction;

namespace BankSystem.Application.Application.Transactions;

public class EnforceTransactionHandler : IRequestHandler<Command>
{
    private readonly IDataBaseContext _context;
    private readonly ICurrentAccountService _currentAccountService;

    public EnforceTransactionHandler(IDataBaseContext context, ICurrentAccountService currentAccountService)
    {
        _context = context;
        _currentAccountService = currentAccountService;
    }

    public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
    {
        Transaction transaction = await _context.Transactions.GetById(request.OrderId);
        Account? account = _currentAccountService.CurrentAccount;

        if (account is null)
            throw new ArgumentException("Account not found");

        transaction.Enforce(account);

        await _context.Transactions.Update(transaction);
        await _context.Accounts.Update(account);

        return Unit.Value;
    }
}