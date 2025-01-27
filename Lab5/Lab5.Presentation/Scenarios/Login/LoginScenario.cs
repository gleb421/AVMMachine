using Lab5.Application.Contracts.Users;
using Lab5.Domain.ValueObjects;
using Spectre.Console;

namespace Lab5.Presentation.Scenarios.Login;

public class LoginScenario : IScenario
{
    private readonly IUserService _userService;

    public LoginScenario(IUserService userService)
    {
        _userService = userService;
    }

    public string Name => "Login User";

    public void Run()
    {
        string accountNumber = AnsiConsole.Ask<string>("Enter your account number");
        int pinCode = AnsiConsole.Ask<int>("Enter your pincode");

        LoginResult result = _userService.Login(accountNumber, new PinCode(pinCode));

        string message = result switch
        {
            LoginResult.Success => "Successful login",
            LoginResult.NotFound => "User not found",
            _ => throw new ArgumentOutOfRangeException(nameof(result)),
        };

        AnsiConsole.WriteLine(message);
    }
}