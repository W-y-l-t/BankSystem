using BankSystem.Application.Application.Abstractions.DataAccess;
using BankSystem.Application.Application.Mapping;
using BankSystem.Domain.Domain.Users;
using MediatR;
using static BankSystem.Application.Application.Contracts.Users.LoginUser;

namespace BankSystem.Application.Application.Users;

public class LoginUserHandler : IRequestHandler<Command, Response>
{
    private readonly IDataBaseContext _context;

    public LoginUserHandler(IDataBaseContext context)
    {
        _context = context;
    }

    public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
    {
        User? user = await _context.Users.FindByAccountProperties(request.Name, request.Password);

        return user is null
            ? new Response(null, false)
            : new Response(user.AsDto(), true);
    }
}