using BankSystem.Application.Application.Abstractions.Services;
using Spectre.Console;

namespace BankSystem.Presentation.Scenarios.System;

public class ChangeUserScenario : IScenario
{
    private readonly ICurrentUserService _currentUserService;

    public ChangeUserScenario(ICurrentUserService currentUserService)
    {
        _currentUserService = currentUserService;
    }

    public string Name => "Change User";

    public async Task Run()
    {
        string confirm = await AnsiConsole.PromptAsync(
            new SelectionPrompt<string>()
                .Title("Are you sure you want to change user?")
                .AddChoices("Yes", "No"));

        if (confirm is "Yes")
        {
            await _currentUserService.ClearContext();
        }
    }
}