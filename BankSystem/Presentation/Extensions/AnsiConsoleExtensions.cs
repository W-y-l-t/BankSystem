using Spectre.Console;

namespace BankSystem.Presentation.Extensions;

public static class AnsiConsoleExtensions
{
    public static void WaitForEnter()
    {
        AnsiConsole.MarkupLine("[gray](Press [underline]Enter[/] to continue)[/]");
        Console.ReadLine();
    }
}