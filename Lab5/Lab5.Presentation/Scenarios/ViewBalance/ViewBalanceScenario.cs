using Lab5.Application.Contracts.Users;
using Lab5.Domain.Models;
using Spectre.Console;

namespace Lab5.Presentation.Scenarios.CheckBalance;

public class ViewBalanceScenario : IScenario
{
    private readonly IUserService _userService;

    public ViewBalanceScenario(IUserService userService)
    {
        _userService = userService;
    }

    public string Name => "View Balance";

    public void Run()
    {
        Money balance = _userService.GetBalance();

        AnsiConsole.WriteLine($"Current balance: {balance.Amount} {balance.Currency}");
    }
}