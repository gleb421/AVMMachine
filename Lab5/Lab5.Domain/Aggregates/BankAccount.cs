using Lab5.Domain.Enteties;
using Lab5.Domain.Models;
using Lab5.Domain.ValueObjects;
using System.Collections.ObjectModel;

namespace Lab5.Domain.Aggregates;

public class BankAccount
{
    public Guid AccountId { get; private set; }

    public string AccountNumber { get; private set; }

    public PinCode PinCode { get; private set; }

    public Money Balance { get; private set; }

    private readonly Collection<Transaction> _transactions;

    public IReadOnlyCollection<Transaction> Transactions => new ReadOnlyCollection<Transaction>(_transactions);

    public BankAccount(Guid id, string accountNumber, PinCode pinCode, Money initialBalance)
    {
        if (string.IsNullOrWhiteSpace(accountNumber))
            throw new ArgumentException("Account number cannot be empty.", nameof(accountNumber));
        AccountId = id;
        AccountNumber = accountNumber;
        PinCode = pinCode;
        Balance = initialBalance;
        _transactions = new Collection<Transaction>();
    }

    public void Deposit(Money amount)
    {
        Balance = Balance.Add(amount);
        AddTransaction(new Transaction(Guid.NewGuid(), DateTime.UtcNow, amount, "Deposit"));
    }

    public void Withdraw(Money amount)
    {
        Balance = Balance.Subtract(amount);
        AddTransaction(new Transaction(
            Guid.NewGuid(),
            DateTime.UtcNow,
            Money.FromDecimal(amount.Amount * 1, amount.Currency),
            "Withdraw"));
    }

    public Money CheckBalance() => Balance;

    public void LoadTransactions(IEnumerable<Transaction> transactions)
    {
        ArgumentNullException.ThrowIfNull(transactions);

        _transactions.Clear();
        foreach (Transaction transaction in transactions)
        {
            _transactions.Add(transaction);
        }
    }

    public Transaction LastTransaction => _transactions.LastOrDefault() ?? throw new InvalidOperationException();

    private void AddTransaction(Transaction transaction)
    {
        _transactions.Add(transaction);
    }
}