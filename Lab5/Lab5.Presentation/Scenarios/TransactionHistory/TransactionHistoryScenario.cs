using Lab5.Application.Contracts.Users;
using Lab5.Domain.Enteties;
using Spectre.Console;

namespace Lab5.Presentation.Scenarios.TransactionHistory;

public class TransactionHistoryScenario : IScenario
{
    private readonly IUserService _userService;

    public TransactionHistoryScenario(IUserService userService)
    {
        _userService = userService;
    }

    public string Name => "Transaction History";

    public void Run()
    {
        IReadOnlyCollection<Transaction> history = _userService.GetTransactionHistory();

        foreach (Transaction transaction in history)
        {
            AnsiConsole.WriteLine($"{transaction.Amount.Amount}, {transaction.Amount.Currency}, {transaction.Date}, {transaction.Description}");
        }
    }
}