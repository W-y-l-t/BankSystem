using MediatR;

namespace BankSystem.Application.Application.Contracts.Accounts;

public static class GetAllAccounts
{
    public record struct Command : IRequest<Response>;

    public record struct Response(IReadOnlyCollection<Guid> Accounts);
}