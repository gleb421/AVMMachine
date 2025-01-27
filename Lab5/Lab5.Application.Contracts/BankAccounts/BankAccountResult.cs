namespace Lab5.Application.Contracts;

public abstract record BankAccountResult
{
    private BankAccountResult() { }

    public sealed record Success : BankAccountResult;

    public sealed record NotEnoughMoney : BankAccountResult;

    public sealed record NoTransaction : BankAccountResult;

    public sealed record AccountAlreadyCreated : BankAccountResult;

    public sealed record WrongConnectionInfo : BankAccountResult;
}