using Lab5.Domain.Aggregates;

namespace Lab5.Application.Abstractions.Repositories;

public interface IUserRepository
{
    User? GetByUserId(Guid userId);

    void Save(User user);

    User? FindByAccountNumber(string userName);
}