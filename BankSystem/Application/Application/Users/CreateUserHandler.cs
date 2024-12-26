using BankSystem.Application.Application.Abstractions.DataAccess;
using BankSystem.Application.Application.Mapping;
using BankSystem.Domain.Domain.Users;
using MediatR;
using static BankSystem.Application.Application.Contracts.Users.CreateUser;

namespace BankSystem.Application.Application.Users;

public class CreateUserHandler : IRequestHandler<Command, Response>
{
    private readonly IDataBaseContext _context;

    public CreateUserHandler(IDataBaseContext context)
    {
        _context = context;
    }

    public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
    {
        if (request.Role is UserRole.Admin)
        {
            if (await CheckIfPasswordIsSystem(request.Password) is false)
            {
                throw new ArgumentException("The administrator password must match one of the system passwords.");
            }
        }

        var user = new User(Guid.NewGuid(), request.Name, request.Role, request.Password);

        await _context.Users.Add(user);

        return new Response(user.AsDto());
    }

    private async Task<bool> CheckIfPasswordIsSystem(string password)
    {
        return await _context.SystemPasswords.FindPassword(password) is not null;
    }
}