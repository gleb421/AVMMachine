using Lab5.Application.Conracts.Users;
using Lab5.Application.Contracts.Users;
using Lab5.Domain.Models;
using Spectre.Console;

namespace Lab5.Presentation.Scenarios.Deposit;

public class DepositScenario : IScenario
{
    private readonly IUserService _userService;

    private readonly ICurrentUserService _currentUser;

    public DepositScenario(IUserService userService, ICurrentUserService currentUser)
    {
        _userService = userService;
        _currentUser = currentUser;
    }

    public string Name => "Deposit Money";

    public void Run()
    {
        if (_currentUser.User != null)
        {
            decimal amount = AnsiConsole.Ask<decimal>("Enter the amount to deposit");
            Money newBalance =
                _userService.Deposit(new Money(amount, _currentUser.User.Account.Balance.Currency));
            AnsiConsole.WriteLine($"Deposit successful. New balance: {newBalance.Amount}, {newBalance.Currency}");
        }
    }
}