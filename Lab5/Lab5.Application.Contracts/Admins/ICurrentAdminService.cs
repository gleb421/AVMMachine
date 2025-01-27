using Lab5.Domain.Enteties;

namespace Lab5.Application.Conracts.Admins;

public interface ICurrentAdminService
{
    Admin? Admin { get; }
}