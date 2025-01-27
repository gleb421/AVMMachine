using Lab5.Application.Conracts.Users;
using Lab5.Application.Contracts.Users;
using Lab5.Presentation.Scenarios.CheckBalance;
using System.Diagnostics.CodeAnalysis;

namespace Lab5.Presentation.Scenarios.ViewBalance;

public class ViewBalanceScenarioProvider : IScenarioProvider
{
    private readonly IUserService _userService;
    private readonly ICurrentUserService _currentUserService;

    public ViewBalanceScenarioProvider(IUserService userService, ICurrentUserService currentUserService)
    {
        _userService = userService;
        _currentUserService = currentUserService;
    }

    public bool TryGetScenario([NotNullWhen(true)] out IScenario? scenario)
    {
        if (_currentUserService.User is null)
        {
            scenario = null;
            return false;
        }

        scenario = new ViewBalanceScenario(_userService);
        return true;
    }
}