using BankSystem.Application.Application.Abstractions.Services;
using BankSystem.Application.Application.Dto;
using BankSystem.Domain.Domain.Transactions;
using MediatR;
using Spectre.Console;
using System.Globalization;
using static BankSystem.Application.Application.Contracts.Accounts.ShowHistory;
using AnsiConsoleExtensions = BankSystem.Presentation.Extensions.AnsiConsoleExtensions;

namespace BankSystem.Presentation.Scenarios.Accounts;

public class ShowHistoryScenario : IScenario
{
    private readonly IMediator _mediator;
    private readonly ICurrentUserService _currentUserService;
    private readonly ICurrentAccountService _currentAccountService;

    public ShowHistoryScenario(
        IMediator mediator,
        ICurrentUserService currentUserService,
        ICurrentAccountService currentAccountService)
    {
        _mediator = mediator;
        _currentUserService = currentUserService;
        _currentAccountService = currentAccountService;
    }

    public string Name => "Show History";

    public async Task Run()
    {
        if (_currentUserService.CurrentUser is null)
        {
            AnsiConsole.MarkupLine("[red]You must logged in to get the history.[/]");
            AnsiConsoleExtensions.WaitForEnter();

            return;
        }

        if (_currentAccountService.CurrentAccount is null)
        {
            AnsiConsole.MarkupLine("[red]You must be in account to get the history.[/]");
            AnsiConsoleExtensions.WaitForEnter();

            return;
        }

        var command = new Command(_currentAccountService.CurrentAccount.Id);
        Response response = await _mediator.Send(command);

        AnsiConsole.MarkupLine($"[white]Transactions history of account {_currentAccountService.CurrentAccount.Id}[/]");
        foreach (TransactionDto transaction in response.Transactions)
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

        AnsiConsoleExtensions.WaitForEnter();
    }
}