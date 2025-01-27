using Lab5.Application.Conracts.Users;
using Lab5.Application.Contracts;
using Lab5.Application.Contracts.Users;
using System.Diagnostics.CodeAnalysis;

namespace Lab5.Presentation.Scenarios.Creation;

public class CreateScenarioProvider : IScenarioProvider
{
    private readonly IUserService _service;
    private readonly ICurrentUserService _currentUser;
    private readonly IBankAccountService _accountService;

    public CreateScenarioProvider(
        IUserService service,
        ICurrentUserService currentUser,
        IBankAccountService accountService)
    {
        _service = service;
        _currentUser = currentUser;
        _accountService = accountService;
    }

    public bool TryGetScenario(
        [NotNullWhen(true)] out IScenario? scenario)
    {
        if (_currentUser.User is not null)
        {
            scenario = null;
            return false;
        }

        scenario = new CreateScenario(_service, _accountService);
        return true;
    }
}