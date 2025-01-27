using Lab5.Application.Contracts;
using Lab5.Application.Contracts.Users;
using Lab5.Domain.ValueObjects;
using Spectre.Console;

namespace Lab5.Presentation.Scenarios.Creation;

public class CreateScenario : IScenario
{
    private readonly IUserService _userService;
    private readonly IBankAccountService _bankAccountService;

    public CreateScenario(IUserService userService, IBankAccountService bankAccountService)
    {
        _userService = userService;
        _bankAccountService = bankAccountService;
    }

    public string Name => "Create account";

    public void Run()
    {
        string accountNumber = AnsiConsole.Ask<string>("Enter your account number");
        int pinCode = AnsiConsole.Ask<int>("Enter your pincode");
        string name = AnsiConsole.Ask<string>("Enter your name");
        _userService.LinkAccount(_bankAccountService.CreateAccount(accountNumber, new PinCode(pinCode)), new Name(name));
    }
}