using Lab5.Domain.Enteties;

namespace Lab5.Application.Conracts.Admins;

public interface IAdminService
{
    AdminLoginResult CheckPassword(string systemPassword);

    void Save(Admin admin);
}