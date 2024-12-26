using BankSystem.Application.Application.Abstractions.Services;
using BankSystem.Presentation.Scenarios;
using BankSystem.Presentation.Scenarios.Accounts;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace BankSystem.Presentation.ScenarioProviders.Accounts;

public class LoginAccountScenarioProvider : IScenarioProvider
{
    private readonly IMediator _mediator;
    private readonly ICurrentUserService _currentUserService;
    private readonly ICurrentAccountService _currentAccountService;

    public LoginAccountScenarioProvider(
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
        if (_currentUserService.CurrentUser is not null && _currentAccountService.CurrentAccount is null)
        {
            scenario = new LoginAccountScenario(_mediator, _currentUserService, _currentAccountService);

            return true;
        }

        scenario = null;

        return false;
    }
}