using BankSystem.Application.Application.Dto;
using BankSystem.Domain.Domain.Accounts;

namespace BankSystem.Application.Application.Mapping;

public static class AccountMapping
{
    public static AccountDto AsDto(this Account account)
    {
        return new AccountDto(
            account.Id,
            account.Balance,
            account.PinCode,
            account.OwnerId,
            account.History);
    }
}