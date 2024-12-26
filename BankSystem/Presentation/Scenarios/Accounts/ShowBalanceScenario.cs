using BankSystem.Application.Application.Abstractions.Services;
using Spectre.Console;
using AnsiConsoleExtensions = BankSystem.Presentation.Extensions.AnsiConsoleExtensions;

namespace BankSystem.Presentation.Scenarios.Accounts;

public class ShowBalanceScenario : IScenario
{
    private readonly ICurrentUserService _currentUserService;
    private readonly ICurrentAccountService _currentAccountService;

    public ShowBalanceScenario(
        ICurrentUserService currentUserService,
        ICurrentAccountService currentAccountService)
    {
        _currentUserService = currentUserService;
        _currentAccountService = currentAccountService;
    }

    public string Name => "Show Balance";

    public Task Run()
    {
        if (_currentUserService.CurrentUser is null)
        {
            AnsiConsole.MarkupLine("[red]You must logged in to get the history.[/]");
            AnsiConsoleExtensions.WaitForEnter();

            return Task.CompletedTask;
        }

        if (_currentAccountService.CurrentAccount is null)
        {
            AnsiConsole.MarkupLine("[red]You must be in account to get the history.[/]");
            AnsiConsoleExtensions.WaitForEnter();

            return Task.CompletedTask;
        }

        AnsiConsole.MarkupLine($"[white]Balance of account {_currentAccountService.CurrentAccount.Balance.Value}[/]");
        AnsiConsoleExtensions.WaitForEnter();

        return Task.CompletedTask;
    }
}