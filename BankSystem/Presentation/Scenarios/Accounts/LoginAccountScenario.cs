using BankSystem.Application.Application.Abstractions.Services;
using BankSystem.Domain.Domain.Accounts;
using BankSystem.Domain.Domain.Common.ValueObjects;
using MediatR;
using Spectre.Console;
using static BankSystem.Application.Application.Contracts.Accounts.LoginAccount;
using AnsiConsoleExtensions = BankSystem.Presentation.Extensions.AnsiConsoleExtensions;

namespace BankSystem.Presentation.Scenarios.Accounts;

public class LoginAccountScenario : IScenario
{
    private readonly IMediator _mediator;
    private readonly ICurrentUserService _currentUserService;
    private readonly ICurrentAccountService _currentAccountService;

    public LoginAccountScenario(
        IMediator mediator,
        ICurrentUserService currentUserService,
        ICurrentAccountService currentAccountService)
    {
        _mediator = mediator;
        _currentUserService = currentUserService;
        _currentAccountService = currentAccountService;
    }

    public string Name => "Log In To Account";

    public async Task Run()
    {
        if (_currentUserService.CurrentUser is null)
        {
            AnsiConsole.MarkupLine("[red]You must be logged in to select an account![/]");
            AnsiConsoleExtensions.WaitForEnter();

            return;
        }

        var accounts = _currentUserService.CurrentUser.Accounts.ToList();

        if (accounts.Count == 0)
        {
            AnsiConsole.MarkupLine("[red]No accounts found for the current user.[/]");
            AnsiConsoleExtensions.WaitForEnter();

            return;
        }

        Guid selectedAccount = await AnsiConsole.PromptAsync(
            new SelectionPrompt<Guid>()
                .Title("Select an account to log in")
                .AddChoices(accounts)
                .UseConverter(account => account.ToString()));

        string pinCode = await AnsiConsole.AskAsync<string>("Enter PIN code for the selected account:");

        var command = new Command(selectedAccount, new PinCode(pinCode));
        Response response = await _mediator.Send(command);

        if (response is { IsAuthenticated: true, Account: not null })
        {
            var currentAccount = new Account(
                response.Account.Id,
                response.Account.PinCode,
                response.Account.OwnerId,
                response.Account.Balance,
                response.Account.History);
            await _currentAccountService.SetCurrentAccount(currentAccount);

            AnsiConsole.MarkupLine($"[green]Successfully logged into account: {response.Account.Id}[/]");
        }
        else
        {
            AnsiConsole.MarkupLine("[red]Invalid Pin Code[/]");
        }

        AnsiConsoleExtensions.WaitForEnter();
    }
}