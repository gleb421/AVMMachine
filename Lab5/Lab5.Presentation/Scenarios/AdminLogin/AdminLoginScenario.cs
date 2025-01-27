using Lab5.Application.Conracts.Admins;
using Spectre.Console;

namespace Lab5.Presentation.Scenarios.AdminLogin;

public class AdminLoginScenario : IScenario
{
    private readonly IAdminService _adminService;

    public AdminLoginScenario(IAdminService adminService)
    {
        _adminService = adminService;
    }

    public string Name => "Login Admin";

    public void Run()
    {
        string systemPassword = AnsiConsole.Ask<string>("Enter your system password");
        AdminLoginResult result = _adminService.CheckPassword(systemPassword);

        string message = result switch
        {
            AdminLoginResult.Success => "Successful login",
            AdminLoginResult.NotFound => ExitWithError("Invalid system password"),
            _ => "User not found",
        };
        AnsiConsole.WriteLine(message);
    }

    private static string ExitWithError(string errorMessage)
    {
        Console.WriteLine(errorMessage);
        Environment.Exit(1);
        return string.Empty;
    }
}