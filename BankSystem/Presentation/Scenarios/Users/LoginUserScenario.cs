using BankSystem.Application.Application.Abstractions.Services;
using BankSystem.Application.Application.Services;
using BankSystem.Domain.Domain.Users;
using MediatR;
using Spectre.Console;
using static BankSystem.Application.Application.Contracts.Users.LoginUser;
using AnsiConsoleExtensions = BankSystem.Presentation.Extensions.AnsiConsoleExtensions;

namespace BankSystem.Presentation.Scenarios.Users;

public class LoginUserScenario : IScenario
{
    private readonly IMediator _mediator;
    private readonly ICurrentUserService _currentUserService;

    public LoginUserScenario(IMediator mediator, ICurrentUserService currentUserService)
    {
        _mediator = mediator;
        _currentUserService = currentUserService;
    }

    public string Name => "Log In";

    public async Task Run()
    {
        string username = await AnsiConsole.AskAsync<string>("Enter your username:");
        string password = await AnsiConsole.AskAsync<string>("Enter your password:");

        string hashPassword = PasswordHasherService.HashPassword(password);

        var command = new Command(username, hashPassword);
        Response response = await _mediator.Send(command);

        if (response is { IsAuthenticated: true, User: not null })
        {
            var currentUser = new User(
                response.User.Id,
                response.User.Name,
                response.User.Role,
                response.User.Password,
                response.User.Accounts);
            await _currentUserService.SetCurrentUser(currentUser);

            AnsiConsole.MarkupLine($"[green]Welcome, {response.User.Name}![/]");
        }
        else
        {
            AnsiConsole.MarkupLine("[red]User not found![/]");
        }

        AnsiConsoleExtensions.WaitForEnter();
    }
}