using Lab5.Application.Conracts.Users;
using Lab5.Domain.Aggregates;

namespace Lab5.Application.Users;

internal class CurrentUserManager : ICurrentUserService
{
    public User? User { get; set; }
}