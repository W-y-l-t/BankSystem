namespace BankSystem.Presentation.Scenarios;

public interface IScenario
{
    string Name { get; }

    Task Run();
}