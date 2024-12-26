using BankSystem.Application.Application.Abstractions.Repositories;
using BankSystem.Domain.Domain.Accounts;
using BankSystem.Domain.Domain.Common.ValueObjects;
using BankSystem.Domain.Domain.Transactions;
using Moq;
using Xunit;

namespace BankSystemTests;

public class Lab5Tests
{
    [Fact]
    public void Transactions_Transaction_DepositSuccess()
    {
        // Arrange
        var account = new Account(
            id: Guid.NewGuid(),
            pinCode: new PinCode("1234"),
            ownerId: Guid.NewGuid());

        var transaction = new Transaction(
            id: Guid.NewGuid(),
            amount: new Money(50),
            transactionType: TransactionType.Deposit,
            accountId: account.Id);

        // Act
        transaction.Enforce(account);

        // Assert
        Assert.Equal(50, account.Balance.Value);
        Assert.Contains(transaction.Id, account.History);
        Assert.Equal(TransactionStatus.Completed, transaction.Status);
    }

    [Fact]
    public void Transactions_Transaction_WithdrawSuccess()
    {
        // Arrange
        var account = new Account(
            id: Guid.NewGuid(),
            pinCode: new PinCode("8329"),
            ownerId: Guid.NewGuid());

        var transactionDeposit = new Transaction(
            id: Guid.NewGuid(),
            amount: new Money(1000),
            transactionType: TransactionType.Deposit,
            accountId: account.Id);

        var transactionWithdraw = new Transaction(
            id: Guid.NewGuid(),
            amount: new Money(7),
            transactionType: TransactionType.Withdraw,
            accountId: account.Id);

        // Act
        transactionDeposit.Enforce(account);
        transactionWithdraw.Enforce(account);

        // Assert
        Assert.Equal(993, account.Balance.Value);
        Assert.Contains(transactionDeposit.Id, account.History);
        Assert.Contains(transactionWithdraw.Id, account.History);
        Assert.Equal(TransactionStatus.Completed, transactionDeposit.Status);
        Assert.Equal(TransactionStatus.Completed, transactionWithdraw.Status);
    }

    [Fact]
    public void Transactions_Transaction_WithdrawRejected()
    {
        // Arrange
        var account = new Account(
            id: Guid.NewGuid(),
            pinCode: new PinCode("9193"),
            ownerId: Guid.NewGuid());

        var transactionDeposit = new Transaction(
            id: Guid.NewGuid(),
            amount: new Money(1000),
            transactionType: TransactionType.Deposit,
            accountId: account.Id);

        var transactionWithdraw = new Transaction(
            id: Guid.NewGuid(),
            amount: new Money(1001),
            transactionType: TransactionType.Withdraw,
            accountId: account.Id);

        // Act
        transactionDeposit.Enforce(account);
        transactionWithdraw.Enforce(account);

        // Assert
        Assert.Equal(1000, account.Balance.Value);
        Assert.Contains(transactionDeposit.Id, account.History);
        Assert.Equal(TransactionStatus.Completed, transactionDeposit.Status);
        Assert.Equal(TransactionStatus.Rejected, transactionWithdraw.Status);
    }

    [Fact]
    public void Repositories_AccountRepository_SaveAccountWithUpdatedBalance()
    {
        // Arrange
        var mockAccountRepository = new Mock<IAccountRepository>();
        var account = new Account(
            id: Guid.NewGuid(),
            pinCode: new PinCode("2340"),
            ownerId: Guid.NewGuid(),
            balance: new Money(100),
            history: Array.Empty<Guid>());

        mockAccountRepository.Setup(repo =>
            repo.FindById(It.IsAny<Guid>())).ReturnsAsync(account);

        var transaction = new Transaction(
            id: Guid.NewGuid(),
            amount: new Money(50),
            transactionType: TransactionType.Withdraw,
            accountId: account.Id);

        // Act
        transaction.Enforce(account);
        mockAccountRepository.Object.Add(account);

        // Assert
        mockAccountRepository.Verify(repo =>
            repo.Add(It.Is<Account>(a => a.Balance.Value == 50)));
    }

    [Fact]
    public async Task Repositories_TransactionRepository_SaveTransaction()
    {
        // Arrange
        var mockTransactionRepository = new Mock<ITransactionRepository>();
        var expectedTransaction = new Transaction(
            id: Guid.NewGuid(),
            amount: new Money(200),
            transactionType: TransactionType.Withdraw,
            accountId: Guid.NewGuid(),
            createdAt: DateTime.UtcNow,
            status: TransactionStatus.Created);

        mockTransactionRepository
            .Setup(repo => repo.GetById(It.IsAny<Guid>()))
            .ReturnsAsync(expectedTransaction);

        // Act
        Transaction result = await mockTransactionRepository.Object.GetById(expectedTransaction.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedTransaction.Id, result.Id);
        Assert.Equal(expectedTransaction.Amount.Value, result.Amount.Value);
        Assert.Equal(expectedTransaction.Type, result.Type);
        Assert.Equal(expectedTransaction.AccountId, result.AccountId);
        Assert.Equal(expectedTransaction.Status, result.Status);
    }
}