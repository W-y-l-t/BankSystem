using BankSystem.Application.Application.Abstractions.Services;
using BankSystem.Presentation.Scenarios;
using BankSystem.Presentation.Scenarios.Users;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace BankSystem.Presentation.ScenarioProviders.Users;

public class LoginUserScenarioProvider : IScenarioProvider
{
    private readonly IMediator _mediator;
    private readonly ICurrentUserService _currentUserService;

    public LoginUserScenarioProvider(IMediator mediator, ICurrentUserService currentUserService)
    {
        _mediator = mediator;
        _currentUserService = currentUserService;
    }

    public bool TryGetScenario([NotNullWhen(true)] out IScenario? scenario)
    {
        if (_currentUserService.CurrentUser is null)
        {
            scenario = new LoginUserScenario(_mediator, _currentUserService);

            return true;
        }

        scenario = null;

        return false;
    }
}