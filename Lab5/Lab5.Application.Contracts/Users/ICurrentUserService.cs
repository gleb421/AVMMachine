using Lab5.Domain.Aggregates;

namespace Lab5.Application.Conracts.Users;

public interface ICurrentUserService
{
    User? User { get; }
}