using Lab5.Domain.Models;
using Lab5.Domain.ValueObjects;

namespace Lab5.Domain.Aggregates;

public class User
{
    public Guid UserId { get; private set; }

    public Name Name { get; private set; }

    public BankAccount Account { get; private set; }

    public User(Guid userId, Name name, BankAccount account)
    {
        UserId = userId;
        Name = name;
        Account = account;
    }

    public void DepositToAccount(Money amount)
    {
        Account.Deposit(amount);
    }

    public void WithdrawFromAccount(Money amount)
    {
        Account.Withdraw(amount);
    }
}