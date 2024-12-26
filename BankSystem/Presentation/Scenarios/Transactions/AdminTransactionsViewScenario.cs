using BankSystem.Application.Application.Abstractions.Services;
using BankSystem.Application.Application.Contracts.Accounts;
using BankSystem.Application.Application.Contracts.Transactions;
using BankSystem.Application.Application.Dto;
using BankSystem.Domain.Domain.Transactions;
using BankSystem.Domain.Domain.Users;
using MediatR;
using Spectre.Console;
using System.Globalization;
using AnsiConsoleExtensions = BankSystem.Presentation.Extensions.AnsiConsoleExtensions;

namespace BankSystem.Presentation.Scenarios.Transactions;

public class AdminTransactionsViewScenario : IScenario
{
    private readonly IMediator _mediator;
    private readonly ICurrentUserService _currentUserService;

    public AdminTransactionsViewScenario(
        IMediator mediator,
        ICurrentUserService currentUserService)
    {
        _mediator = mediator;
        _currentUserService = currentUserService;
    }

    public string Name => "Show Transactions History";

    public async Task Run()
    {
        if (_currentUserService.CurrentUser?.Role is not UserRole.Admin)
        {
            AnsiConsole.MarkupLine("[red]You must be logged as admin.[/]");
            AnsiConsoleExtensions.WaitForEnter();

            return;
        }

        var accountsCommand = default(GetAllAccounts.Command);
        GetAllAccounts.Response accounts = await _mediator.Send(accountsCommand);

        foreach (Guid accountId in accounts.Accounts)
        {
            var historyCommand = new GetAllTransactions.Command(accountId);
            GetAllTransactions.Response transactions = await _mediator.Send(historyCommand);

            AnsiConsole.MarkupLine($"[white]{accountId} account transactions[/]");
            foreach (TransactionDto transaction in transactions.Transactions)
            {
                switch (transaction.Type)
                {
                    case TransactionType.Deposit:
                        AnsiConsole.MarkupLine(
                            $"[green]{transaction.Status.ToString().ToUpper(CultureInfo.CurrentCulture)}  " +
                            $"{transaction.Id}  {transaction.Type}  " +
                            $"{transaction.Amount.Value}  {transaction.Date}[/]");
                        break;
                    case TransactionType.Withdraw:
                        AnsiConsole.MarkupLine(
                            $"[red]{transaction.Status.ToString().ToUpper(CultureInfo.CurrentCulture)}  " +
                            $"{transaction.Id}  {transaction.Type}  " +
                            $"{transaction.Amount.Value}  {transaction.Date}[/]");
                        break;
                    default:
                        AnsiConsole.MarkupLine(
                            $"[grey]{transaction.Status.ToString().ToUpper(CultureInfo.CurrentCulture)}  " +
                            $"{transaction.Id}  {transaction.Type}  " +
                            $"{transaction.Amount.Value}  {transaction.Date}[/]");
                        break;
                }
            }

            AnsiConsole.WriteLine();
        }

        AnsiConsoleExtensions.WaitForEnter();
    }
}