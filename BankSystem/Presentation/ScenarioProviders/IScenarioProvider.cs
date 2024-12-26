using BankSystem.Presentation.Scenarios;
using System.Diagnostics.CodeAnalysis;

namespace BankSystem.Presentation.ScenarioProviders;

public interface IScenarioProvider
{
    bool TryGetScenario([NotNullWhen(true)] out IScenario? scenario);
}