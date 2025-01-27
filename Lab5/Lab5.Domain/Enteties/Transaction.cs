using Lab5.Domain.Models;

namespace Lab5.Domain.Enteties;

public class Transaction
{
    public Guid Id { get; private set; }

    public DateTime Date { get; private set; }

    public Money Amount { get; private set; }

    public string Description { get; private set; }

    public Transaction(Guid id, DateTime date, Money amount, string description)
    {
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Описание не может быть пустым.", nameof(description));
        Id = id;
        Date = date;
        Amount = amount;
        Description = description;
    }

    public override string ToString()
    {
        return $"{Date}: {Description} {Amount}";
    }
}