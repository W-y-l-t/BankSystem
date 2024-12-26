using BankSystem.Presentation.ScenarioProviders;
using Spectre.Console;

namespace BankSystem.Presentation.Scenarios;

public class ScenarioRunner
{
    private readonly IEnumerable<IScenarioProvider> _providers;

    public ScenarioRunner(IEnumerable<IScenarioProvider> providers)
    {
        _providers = providers;
    }

    public async Task Run()
    {
        IEnumerable<IScenario> scenarios = GetScenarios();

        SelectionPrompt<IScenario> selector = new SelectionPrompt<IScenario>()
            .Title("Select action")
            .AddChoices(scenarios)
            .UseConverter(x => x.Name);

        IScenario scenario = await AnsiConsole.PromptAsync(selector);

        await scenario.Run();
    }

    private IEnumerable<IScenario> GetScenarios()
    {
        var scenarios = new SortedSet<IScenario>(
            Comparer<IScenario>.Create(
                (a, b) => string.Compare(b.Name, a.Name, StringComparison.Ordinal)));
        foreach (IScenarioProvider provider in _providers)
        {
            if (provider.TryGetScenario(out IScenario? scenario))
                scenarios.Add(scenario);
        }

        return scenarios.AsEnumerable();
    }
}