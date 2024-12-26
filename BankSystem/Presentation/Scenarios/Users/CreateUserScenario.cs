using BankSystem.Application.Application.Services;
using BankSystem.Domain.Domain.Users;
using MediatR;
using Spectre.Console;
using static BankSystem.Application.Application.Contracts.Users.CreateUser;
using AnsiConsoleExtensions = BankSystem.Presentation.Extensions.AnsiConsoleExtensions;

namespace BankSystem.Presentation.Scenarios.Users;

public class CreateUserScenario : IScenario
{
    private readonly IMediator _mediator;

    public CreateUserScenario(IMediator mediator)
    {
        _mediator = mediator;
    }

    public string Name => "Create User";

    public async Task Run()
    {
        string name = await AnsiConsole.AskAsync<string>("Enter user name:");
        string password = await AnsiConsole.AskAsync<string>("Enter password:");
        string passwordConfirm = await AnsiConsole.AskAsync<string>("Enter password confirmation:");

        if (password != passwordConfirm)
        {
            AnsiConsole.MarkupLine("[red]Passwords do not match![/]");
            AnsiConsoleExtensions.WaitForEnter();

            return;
        }

        UserRole role = await AnsiConsole.PromptAsync(
            new SelectionPrompt<UserRole>()
                .Title("Select user role:")
                .AddChoices(UserRole.Admin, UserRole.Customer));

        string hashPassword = PasswordHasherService.HashPassword(password);

        try
        {
            var command = new Command(name, role, hashPassword);
            Response response = await _mediator.Send(command);

            AnsiConsole.MarkupLine($"[green]User created: {response.User.Name}[/]");
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