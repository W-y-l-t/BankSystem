using BankSystem.Application.Application.Abstractions.Services;
using BankSystem.Presentation.Scenarios;
using BankSystem.Presentation.Scenarios.Accounts;
using System.Diagnostics.CodeAnalysis;

namespace BankSystem.Presentation.ScenarioProviders.Accounts;

public class ShowBalanceScenarioProvider : IScenarioProvider
{
    private readonly ICurrentUserService _currentUserService;
    private readonly ICurrentAccountService _currentAccountService;

    public ShowBalanceScenarioProvider(
        ICurrentUserService currentUserService,
        ICurrentAccountService currentAccountService)
    {
        _currentUserService = currentUserService;
        _currentAccountService = currentAccountService;
    }

    public bool TryGetScenario([NotNullWhen(true)] out IScenario? scenario)
    {
        if (_currentUserService.CurrentUser is not null && _currentAccountService.CurrentAccount is not null)
        {
            scenario = new ShowBalanceScenario(_currentUserService, _currentAccountService);

            return true;
        }

        scenario = null;

        return false;
    }
}