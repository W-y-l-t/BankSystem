using BankSystem.Presentation.Scenarios;
using BankSystem.Presentation.Scenarios.System;
using System.Diagnostics.CodeAnalysis;

namespace BankSystem.Presentation.ScenarioProviders.System;

public class ExitScenarioProvider : IScenarioProvider
{
    public bool TryGetScenario([NotNullWhen(true)] out IScenario? scenario)
    {
        scenario = new ExitScenario();

        return true;
    }
}