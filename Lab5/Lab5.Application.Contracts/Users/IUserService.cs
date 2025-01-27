using Lab5.Domain.Aggregates;
using Lab5.Domain.Enteties;
using Lab5.Domain.Models;
using Lab5.Domain.ValueObjects;

namespace Lab5.Application.Contracts.Users;

public interface IUserService
{
    LoginResult Login(string accountNumber, PinCode pinCode);

    Money Withdraw(Money amount);

    Money Deposit(Money amount);

    Money GetBalance();

    void LinkAccount(BankAccount bankAccount, Name name);

    IReadOnlyCollection<Transaction> GetTransactionHistory();
}