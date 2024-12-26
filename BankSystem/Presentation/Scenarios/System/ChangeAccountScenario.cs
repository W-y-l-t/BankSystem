using BankSystem.Application.Application.Abstractions.Services;
using Spectre.Console;

namespace BankSystem.Presentation.Scenarios.System;

public class ChangeAccountScenario : IScenario
{
    private readonly ICurrentAccountService _currentAccountService;

    public ChangeAccountScenario(ICurrentAccountService currentAccountService)
    {
        _currentAccountService = currentAccountService;
    }

    public string Name => "Change Account";

    public async Task Run()
    {
        string confirm = await AnsiConsole.PromptAsync(
            new SelectionPrompt<string>()
                .Title("Are you sure you want to change account?")
                .AddChoices("Yes", "No"));

        if (confirm is "Yes")
        {
            await _currentAccountService.ClearContext();
        }
    }
}