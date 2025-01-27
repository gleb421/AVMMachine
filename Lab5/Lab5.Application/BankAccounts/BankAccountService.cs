using Lab5.Application.Abstractions.Repositories;
using Lab5.Application.Contracts;
using Lab5.Domain.Aggregates;
using Lab5.Domain.Enteties;
using Lab5.Domain.Models;
using Lab5.Domain.ValueObjects;

namespace Lab5.Application.BankAccounts;

public class BankAccountService : IBankAccountService
{
    private readonly IBankAccountRepository _repository;

    private readonly ITransactionRepository _transactionRepository;

    public BankAccountService(IBankAccountRepository repository, ITransactionRepository transactionRepository)
    {
        _repository = repository;
        _transactionRepository = transactionRepository;
    }

    public BankAccountResult Withdraw(Guid accountId, int pinCode, Money amount)
    {
        BankAccount account = AuthenticateAccount(accountId, pinCode);
        if (account.CheckBalance().Amount < amount.Amount)
        {
            return new BankAccountResult.NotEnoughMoney();
        }

        account.Withdraw(amount);
        _repository.Save(account);
        _transactionRepository.Save(account.LastTransaction, accountId);
        return new BankAccountResult.Success();
    }

    public BankAccountResult Deposit(Guid accountId, int pinCode, Money amount)
    {
        BankAccount account = AuthenticateAccount(accountId, pinCode);
        account.Deposit(amount);
        _repository.Save(account);
        _transactionRepository.Save(account.LastTransaction, accountId);
        return new BankAccountResult.Success();
    }

    public Money GetBalance(Guid accountId, int pinCode)
    {
        BankAccount account = AuthenticateAccount(accountId, pinCode);
        return account.CheckBalance();
    }

    public IReadOnlyCollection<Transaction> GetTransactionHistory(Guid accountId, int pinCode)
    {
        BankAccount account = AuthenticateAccount(accountId, pinCode);
        return account.Transactions;
    }

    public BankAccount CreateAccount(string accountNumber, PinCode pinCode)
    {
        if (string.IsNullOrWhiteSpace(accountNumber))
        {
            throw new ArgumentException("Account number cannot be null or empty.", nameof(accountNumber));
        }

        var newAccount = new BankAccount(
            Guid.NewGuid(),
            accountNumber,
            pinCode,
            new Money(0, "RUB"));

        _repository.Save(newAccount);
        return newAccount;
    }

    private BankAccount AuthenticateAccount(Guid accountId, int pinCode)
    {
        BankAccount? account = _repository.GetByAccountId(accountId);
        if (account == null || account.PinCode.Pin != pinCode)
            throw new UnauthorizedAccessException("Invalid account number or PIN code.");
        return account;
    }
}