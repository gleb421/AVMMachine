using Lab5.Domain.Aggregates;
using Lab5.Domain.Enteties;
using Lab5.Domain.Models;
using Lab5.Domain.ValueObjects;

namespace Lab5.Application.Contracts;

public interface IBankAccountService
{
    BankAccountResult Withdraw(Guid accountId, int pinCode, Money amount);

    BankAccountResult Deposit(Guid accountId, int pinCode, Money amount);

    Money GetBalance(Guid accountId, int pinCode);

    IReadOnlyCollection<Transaction> GetTransactionHistory(Guid accountId, int pinCode);

    BankAccount CreateAccount(string accountNumber, PinCode pinCode);
}