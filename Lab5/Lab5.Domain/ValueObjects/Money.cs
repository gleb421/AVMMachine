namespace Lab5.Domain.Models;

public class Money
{
    public decimal Amount { get; private set; }

    public string Currency { get; private set; }

    public Money(decimal amount, string currency)
    {
        if (amount < 0)
            throw new ArgumentException("Amount cannot be negative.", nameof(amount));

        if (string.IsNullOrWhiteSpace(currency))
            throw new ArgumentException("Currency cannot be empty.", nameof(currency));

        Amount = amount;
        Currency = currency;
    }

    public static Money Zero(string currency) => new Money(0, currency);

    public static Money FromDecimal(decimal amount, string currency) => new Money(amount, currency);

    public Money Add(Money other)
    {
        if (Currency != other.Currency)
            throw new InvalidOperationException("Cannot add amounts with different currencies.");

        return new Money(Amount + other.Amount, Currency);
    }

    public Money Subtract(Money other)
    {
        if (Currency != other.Currency)
            throw new InvalidOperationException("Cannot subtract amounts with different currencies.");

        if (Amount < other.Amount)
            throw new InvalidOperationException("Insufficient funds.");

        return new Money(Amount - other.Amount, Currency);
    }
}