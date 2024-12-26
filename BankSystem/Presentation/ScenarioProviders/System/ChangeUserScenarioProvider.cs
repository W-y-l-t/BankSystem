using BankSystem.Application.Application.Abstractions.Services;
using BankSystem.Presentation.Scenarios;
using BankSystem.Presentation.Scenarios.System;
using System.Diagnostics.CodeAnalysis;

namespace BankSystem.Presentation.ScenarioProviders.System;

public class ChangeUserScenarioProvider : IScenarioProvider
{
    private readonly ICurrentUserService _currentUserService;
    private readonly ICurrentAccountService _currentAccountService;

    public ChangeUserScenarioProvider(
        ICurrentUserService currentUserService,
        ICurrentAccountService currentAccountService)
    {
        _currentUserService = currentUserService;
        _currentAccountService = currentAccountService;
    }

    public bool TryGetScenario([NotNullWhen(true)] out IScenario? scenario)
    {
        if (_currentUserService.CurrentUser is not null && _currentAccountService.CurrentAccount is null)
        {
            scenario = new ChangeUserScenario(_currentUserService);

            return true;
        }

        scenario = null;

        return false;
    }
}