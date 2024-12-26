using BankSystem.Application.Application.Abstractions.Services;
using BankSystem.Domain.Domain.Common.ValueObjects;
using BankSystem.Domain.Domain.Transactions;
using MediatR;
using Spectre.Console;
using static BankSystem.Application.Application.Contracts.Transactions.CreateTransaction;
using AnsiConsoleExtensions = BankSystem.Presentation.Extensions.AnsiConsoleExtensions;

namespace BankSystem.Presentation.Scenarios.Transactions;

public class CreateTransactionScenario : IScenario
{
    private readonly IMediator _mediator;
    private readonly ICurrentUserService _currentUserService;
    private readonly ICurrentAccountService _currentAccountService;

    public CreateTransactionScenario(
        IMediator mediator,
        ICurrentUserService currentUserService,
        ICurrentAccountService currentAccountService)
    {
        _mediator = mediator;
        _currentUserService = currentUserService;
        _currentAccountService = currentAccountService;
    }

    public string Name => "Create Transaction";

    public async Task Run()
    {
        if (_currentUserService.CurrentUser is null)
        {
            AnsiConsole.MarkupLine("[red]You must be logged in to create a transaction.[/]");
            AnsiConsoleExtensions.WaitForEnter();

            return;
        }

        if (_currentAccountService.CurrentAccount is null)
        {
            AnsiConsole.MarkupLine("[red]You must be in account to create a transaction.[/]");
            AnsiConsoleExtensions.WaitForEnter();

            return;
        }

        decimal amount = await AnsiConsole.AskAsync<decimal>("Enter the transaction amount:");
        TransactionType type = await AnsiConsole.PromptAsync(
            new SelectionPrompt<TransactionType>()
                .Title("Select transaction type:")
                .AddChoices(TransactionType.Deposit, TransactionType.Withdraw));

        try
        {
            var command = new Command(new Money(amount), type, _currentAccountService.CurrentAccount.Id);
            Response response = await _mediator.Send(command);
            AnsiConsole.MarkupLine($"[green]Transaction created: {response.Transaction.Id}[/]");
        }
        catch (ArgumentException e)
        {
            AnsiConsole.MarkupLine($"[red]{e.Message}[/]");
        }

        AnsiConsoleExtensions.WaitForEnter();
    }
}