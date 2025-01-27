using Lab5.Application.Abstractions.Repositories;
using Lab5.Application.Conracts.Admins;
using Lab5.Domain.Enteties;

namespace Lab5.Application.Admins;

public class AdminService : IAdminService
{
    private readonly IAdminRepository _adminRepository;
    private readonly CurrentAdminManager _currentAdminManager;

    public AdminService(IAdminRepository adminRepository, CurrentAdminManager currentAdminManager)
    {
        _adminRepository = adminRepository;
        _currentAdminManager = currentAdminManager;
    }

    public AdminLoginResult CheckPassword(string systemPassword)
    {
        if (_currentAdminManager.Admin != null && _currentAdminManager.Admin.ValidatePassword(systemPassword))
        {
            return new AdminLoginResult.Success();
        }

        return new AdminLoginResult.NotFound();
    }

    public void Save(Admin admin)
    {
        _adminRepository.Save(admin);
    }
}