namespace BankSystem.Domain.Domain.Users;

public class User
{
    private readonly List<Guid> _accounts;

    public User(Guid id, string name, UserRole role, string password)
    {
        Id = id;
        Name = name;
        Role = role;
        Password = password;
        _accounts = [];
    }

    public User(Guid id, string name, UserRole role, string password, IReadOnlyCollection<Guid> accounts)
        : this(id, name, role, password)
    {
        _accounts = [.. accounts];
    }

    public Guid Id { get; }

    public string Name { get; }

    public UserRole Role { get; }

    public string Password { get; }

    public IReadOnlyCollection<Guid> Accounts => _accounts;

    public void AddAccount(Guid accountId)
    {
        _accounts.Add(accountId);
    }
}