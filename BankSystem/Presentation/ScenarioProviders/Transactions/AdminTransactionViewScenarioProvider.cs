using BankSystem.Application.Application.Abstractions.Services;
using BankSystem.Domain.Domain.Users;
using BankSystem.Presentation.Scenarios;
using BankSystem.Presentation.Scenarios.Transactions;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace BankSystem.Presentation.ScenarioProviders.Transactions;

public class AdminTransactionViewScenarioProvider : IScenarioProvider
{
    private readonly IMediator _mediator;
    private readonly ICurrentUserService _currentUserService;
    private readonly ICurrentAccountService _currentAccountService;

    public AdminTransactionViewScenarioProvider(
        IMediator mediator,
        ICurrentUserService currentUserService,
        ICurrentAccountService currentAccountService)
    {
        _mediator = mediator;
        _currentUserService = currentUserService;
        _currentAccountService = currentAccountService;
    }

    public bool TryGetScenario([NotNullWhen(true)] out IScenario? scenario)
    {
        if (_currentUserService.CurrentUser is not null &&
            _currentAccountService.CurrentAccount is null &&
            _currentUserService.CurrentUser.Role is UserRole.Admin)
        {
            scenario = new AdminTransactionsViewScenario(_mediator, _currentUserService);

            return true;
        }

        scenario = null;

        return false;
    }
}