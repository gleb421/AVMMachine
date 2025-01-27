namespace Lab5.Domain.ValueObjects;

public class PinCode
{
    public int Pin { get; private set; }

    public PinCode(int pin)
    {
        Pin = pin;
        if (Pin < 1000 || Pin > 9999)
            throw new ArgumentException("Pin must be between 1000 and 9999");
    }
}