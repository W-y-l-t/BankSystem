using Spectre.Console;

namespace BankSystem.Presentation.Scenarios.System;

public class ExitScenario : IScenario
{
    public string Name => "(Exit Program)";

    public async Task Run()
    {
        string confirm = await AnsiConsole.PromptAsync(
            new SelectionPrompt<string>()
                .Title("Are you sure you want to exit?")
                .AddChoices("Yes", "No"));

        if (confirm is "Yes")
        {
            Environment.Exit(0);
        }
    }
}