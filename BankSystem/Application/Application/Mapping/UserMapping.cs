using BankSystem.Application.Application.Dto;
using BankSystem.Domain.Domain.Users;

namespace BankSystem.Application.Application.Mapping;

public static class UserMapping
{
    public static UserDto AsDto(this User user)
    {
        return new UserDto(user.Id, user.Role, user.Name, user.Password, user.Accounts);
    }
}