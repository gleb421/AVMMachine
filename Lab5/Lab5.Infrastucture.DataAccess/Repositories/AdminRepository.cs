using Lab5.Application.Abstractions.Repositories;
using Lab5.Domain.Enteties;
using Lab5.Infrastucture.Mappers.Admins;

namespace Lab5.Infrastucture.Repositories;

public class AdminRepository : IAdminRepository
{
    private readonly IAdminDataMapper _dataMapper;

    public AdminRepository(IAdminDataMapper dataMapper)
    {
        _dataMapper = dataMapper;
    }

    public void Save(Admin admin)
    {
        _dataMapper.Save(admin);
    }

    public Admin? GetByAccountId(Guid accountId)
    {
        return _dataMapper.GetAdminById(accountId);
    }
}