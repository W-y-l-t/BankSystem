using BankSystem.Domain.Domain.Common.ValueObjects;

namespace BankSystem.Application.Application.Dto;

public record AccountDto(
    Guid Id,
    Money Balance,
    PinCode PinCode,
    Guid OwnerId,
    IReadOnlyCollection<Guid> History);