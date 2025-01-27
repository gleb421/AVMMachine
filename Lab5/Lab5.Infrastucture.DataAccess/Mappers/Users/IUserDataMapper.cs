using Lab5.Domain.Aggregates;

namespace Lab5.Infrastucture.Mappers.Users;

public interface IUserDataMapper
{
    void Save(User user);

    User? GetById(Guid userId);

    User? FindUserAndAccount(string accountNumber);
}