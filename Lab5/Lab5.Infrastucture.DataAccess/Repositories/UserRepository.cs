using Lab5.Application.Abstractions.Repositories;
using Lab5.Domain.Aggregates;
using Lab5.Infrastucture.Mappers.Users;

namespace Lab5.Infrastucture.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IUserDataMapper _dataMapper;

    public UserRepository(IUserDataMapper dataMapper)
    {
        _dataMapper = dataMapper;
    }

    public void Save(User user)
    {
         _dataMapper.Save(user);
    }

    public User? GetByUserId(Guid userId)
    {
        return _dataMapper.GetById(userId);
    }

    public User? FindByAccountNumber(string userName)
    {
        return _dataMapper.FindUserAndAccount(userName);
    }
}