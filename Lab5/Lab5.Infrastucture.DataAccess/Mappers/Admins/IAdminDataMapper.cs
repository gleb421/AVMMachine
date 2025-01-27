using Lab5.Domain.Enteties;

namespace Lab5.Infrastucture.Mappers.Admins;

public interface IAdminDataMapper
{
    Admin? GetAdminById(Guid adminId);

    void Save(Admin admin);
}