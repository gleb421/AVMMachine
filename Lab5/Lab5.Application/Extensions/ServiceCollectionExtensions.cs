using Lab5.Application.Admins;
using Lab5.Application.BankAccounts;
using Lab5.Application.Conracts.Admins;
using Lab5.Application.Conracts.Users;
using Lab5.Application.Contracts;
using Lab5.Application.Contracts.Users;
using Lab5.Application.Users;
using Microsoft.Extensions.DependencyInjection;

namespace Lab5.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection collection)
    {
        collection.AddScoped<IUserService, UserService>();
        collection.AddScoped<IBankAccountService, BankAccountService>();
        collection.AddScoped<IAdminService, AdminService>();
        collection.AddScoped<CurrentUserManager>();
        collection.AddScoped<ICurrentUserService>(
            p => p.GetRequiredService<CurrentUserManager>());
        collection.AddScoped<CurrentAdminManager>();
        collection.AddScoped<ICurrentAdminService>(
            p => p.GetRequiredService<CurrentAdminManager>());
        return collection;
    }
}