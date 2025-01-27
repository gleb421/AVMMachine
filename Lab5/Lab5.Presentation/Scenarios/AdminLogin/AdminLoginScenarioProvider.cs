using Lab5.Application.Conracts.Admins;
using System.Diagnostics.CodeAnalysis;

namespace Lab5.Presentation.Scenarios.AdminLogin;

public class AdminLoginScenarioProvider : IScenarioProvider
{
    private readonly IAdminService _adminService;

    private readonly ICurrentAdminService _currentAdminService;

    public AdminLoginScenarioProvider(IAdminService adminService, ICurrentAdminService currentAdminService)
    {
        _adminService = adminService;
        _currentAdminService = currentAdminService;
    }

    public bool TryGetScenario([NotNullWhen(true)] out IScenario? scenario)
    {
        if (_currentAdminService.Admin is null)
        {
            scenario = new AdminLoginScenario(_adminService);
            return true;
        }

        scenario = null;
        return false;
    }
}