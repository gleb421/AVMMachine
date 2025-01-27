using Lab5.Application.Conracts.Admins;
using Lab5.Domain.Enteties;

namespace Lab5.Application.Admins;

public class CurrentAdminManager : ICurrentAdminService
{
    public Admin? Admin { get; set; }
}