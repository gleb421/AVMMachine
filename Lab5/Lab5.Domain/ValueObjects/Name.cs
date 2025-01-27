namespace Lab5.Domain.ValueObjects;

public class Name
{
    public string FirstName { get; private set; }

    public Name(string firstName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(firstName));
        FirstName = firstName;
    }
}