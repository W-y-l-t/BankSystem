using BankSystem.Domain.Domain.Common.ValueObjects;
using BankSystem.Domain.Domain.Transactions;

namespace BankSystem.Application.Application.Dto;

public record TransactionDto(
    Guid Id,
    Money Amount,
    TransactionType Type,
    DateTime Date,
    Guid AccountId,
    TransactionStatus Status);