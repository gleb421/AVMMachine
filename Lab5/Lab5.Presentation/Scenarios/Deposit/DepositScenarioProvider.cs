using Lab5.Application.Conracts.Users;
using Lab5.Application.Contracts.Users;
using System.Diagnostics.CodeAnalysis;

namespace Lab5.Presentation.Scenarios.Deposit;

public class DepositScenarioProvider : IScenarioProvider
{
    private readonly IUserService _userService;

    private readonly ICurrentUserService _currentUserService;

    public DepositScenarioProvider(IUserService userService, ICurrentUserService currentUserService)
    {
        _userService = userService;
        _currentUserService = currentUserService;
    }

    public bool TryGetScenario([NotNullWhen(true)] out IScenario? scenario)
    {
        if (_currentUserService.User != null)
        {
            scenario = new DepositScenario(_userService, _currentUserService);
            return true;
        }

        scenario = null;
        return false;
    }
}