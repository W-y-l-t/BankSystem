using BankSystem.Application.Application.Abstractions.Services;
using BankSystem.Application.Application.Contracts.Transactions;
using BankSystem.Domain.Domain.Transactions;
using MediatR;
using Spectre.Console;
using AnsiConsoleExtensions = BankSystem.Presentation.Extensions.AnsiConsoleExtensions;

namespace BankSystem.Presentation.Scenarios.Transactions;

public class EnforceTransactionScenario : IScenario
{
    private readonly IMediator _mediator;
    private readonly ICurrentUserService _currentUserService;
    private readonly ICurrentAccountService _currentAccountService;

    public EnforceTransactionScenario(
        IMediator mediator,
        ICurrentUserService currentUserService,
        ICurrentAccountService currentAccountService)
    {
        _mediator = mediator;
        _currentUserService = currentUserService;
        _currentAccountService = currentAccountService;
    }

    public string Name => "Confirm Unfulfilled Transaction";

    public async Task Run()
    {
        if (_currentUserService.CurrentUser is null)
        {
            AnsiConsole.MarkupLine("[red]You must be logged in to confirm a transaction.[/]");
            AnsiConsoleExtensions.WaitForEnter();

            return;
        }

        if (_currentAccountService.CurrentAccount is null)
        {
            AnsiConsole.MarkupLine("[red]You must be in account to confirm a transaction.[/]");
            AnsiConsoleExtensions.WaitForEnter();

            return;
        }

        var getAllTransactionsCommand = new GetAllTransactions.Command(_currentAccountService.CurrentAccount.Id);
        GetAllTransactions.Response getAllTransactionsResponse = await _mediator.Send(getAllTransactionsCommand);

        Guid[] unfulfilledTransactions = getAllTransactionsResponse.Transactions
            .Select(t => t)
            .Where(t => t.Status is TransactionStatus.Created)
            .Select(t => t.Id).ToArray();

        if (unfulfilledTransactions.Length == 0)
        {
            AnsiConsole.MarkupLine("[red]There are no unconfirmed transactions[/]");
            AnsiConsoleExtensions.WaitForEnter();

            return;
        }

        Guid selectedTransaction = await AnsiConsole.PromptAsync(
            new SelectionPrompt<Guid>()
                .Title("Select a transaction you want to confirm")
                .AddChoices(unfulfilledTransactions)
                .UseConverter(transaction => transaction.ToString()));

        var getSelectedTransactionCommand = new GetTransaction.Command(selectedTransaction);
        GetTransaction.Response selectedTransactionInfo = await _mediator.Send(getSelectedTransactionCommand);

        string confirm = await AnsiConsole.PromptAsync(
            new SelectionPrompt<string>()
                .Title($"Are you sure you want to {selectedTransactionInfo.Transaction.Type.ToString()} " +
                       $"{selectedTransactionInfo.Transaction.Amount.Value}?")
                .AddChoices("Yes", "No"));

        if (confirm == "No")
        {
            return;
        }

        var enforceCommand = new EnforceTransaction.Command(selectedTransaction);
        await _mediator.Send(enforceCommand);

        var getTransactionToCheckStatusCommand = new GetTransaction.Command(selectedTransaction);
        GetTransaction.Response getTransactionToCheckStatusResponse =
            await _mediator.Send(getTransactionToCheckStatusCommand);
        TransactionStatus enforcedTransactionStatus = getTransactionToCheckStatusResponse.Transaction.Status;

        AnsiConsole.MarkupLine(enforcedTransactionStatus is TransactionStatus.Completed
            ? $"[green]Transaction executed successfully[/]"
            : $"[red]Transaction executed unsuccessfully[/]");

        AnsiConsoleExtensions.WaitForEnter();
    }
}