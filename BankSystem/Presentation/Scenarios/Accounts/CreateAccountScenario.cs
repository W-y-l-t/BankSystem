using BankSystem.Application.Application.Abstractions.Services;
using BankSystem.Domain.Domain.Common.ValueObjects;
using MediatR;
using Spectre.Console;
using static BankSystem.Application.Application.Contracts.Accounts.CreateAccount;
using AnsiConsoleExtensions = BankSystem.Presentation.Extensions.AnsiConsoleExtensions;

namespace BankSystem.Presentation.Scenarios.Accounts;

public class CreateAccountScenario : IScenario
{
    private readonly IMediator _mediator;
    private readonly ICurrentUserService _currentUserService;

    public CreateAccountScenario(IMediator mediator, ICurrentUserService currentUserService)
    {
        _mediator = mediator;
        _currentUserService = currentUserService;
    }

    public string Name => "Create Account";

    public async Task Run()
    {
        if (_currentUserService.CurrentUser is null)
        {
            AnsiConsole.MarkupLine("[red]You must be logged in to create an account.[/]");
            AnsiConsoleExtensions.WaitForEnter();

            return;
        }

        string pinCode = await AnsiConsole.AskAsync<string>("Enter Pin Code:");

        try
        {
            var command = new Command(new PinCode(pinCode), _currentUserService.CurrentUser.Id);
            Response response = await _mediator.Send(command);

            AnsiConsole.MarkupLine($"[green]Account created: {response.Account.Id}[/]");
        }
        catch (ArgumentException e)
        {
            AnsiConsole.MarkupLine($"[red]{e.Message}[/]");
        }
        finally
        {
            AnsiConsoleExtensions.WaitForEnter();
        }
    }
}