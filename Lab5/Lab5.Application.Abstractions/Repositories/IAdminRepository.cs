using Lab5.Domain.Enteties;

namespace Lab5.Application.Abstractions.Repositories;

public interface IAdminRepository
{
    void Save(Admin admin);

    Admin? GetByAccountId(Guid accountId);
}