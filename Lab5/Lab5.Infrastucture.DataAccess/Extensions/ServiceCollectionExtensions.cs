using Itmo.Dev.Platform.Common.Extensions;
using Itmo.Dev.Platform.Postgres.Connection;
using Itmo.Dev.Platform.Postgres.Extensions;
using Itmo.Dev.Platform.Postgres.Models;
using Itmo.Dev.Platform.Postgres.Plugins;
using Lab5.Application.Abstractions.Repositories;
using Lab5.Infrastucture.Mappers.Admins;
using Lab5.Infrastucture.Mappers.BankAccounts;
using Lab5.Infrastucture.Mappers.Transactions;
using Lab5.Infrastucture.Mappers.Users;
using Lab5.Infrastucture.Plugins;
using Lab5.Infrastucture.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Lab5.Infrastucture.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureDataAccess(
        this IServiceCollection collection,
        Action<PostgresConnectionConfiguration> configuration)
    {
        collection.AddPlatform();
        collection.AddPlatformPostgres(builder => builder.Configure(configuration));

        collection.AddPlatformMigrations(typeof(ServiceCollectionExtensions).Assembly);

        collection.AddSingleton<IDataSourcePlugin, MappingPlugin>();

        collection.AddScoped<IUserDataMapper>(provider =>
            new UserDataMapper(provider.GetService<IPostgresConnectionProvider>() ??
                               throw new InvalidOperationException()));

        collection.AddScoped<IAdminDataMapper>(provider =>
            new AdminDataMapper(provider.GetService<IPostgresConnectionProvider>() ??
                                throw new InvalidOperationException()));

        collection.AddScoped<IBankAccountDataMapper, BankAccountDataMapper>(provider =>
            new BankAccountDataMapper(provider.GetService<IPostgresConnectionProvider>() ??
                                      throw new InvalidOperationException()));

        collection.AddScoped<ITransactionDataMapper, TransactionDataMapper>(provider =>
            new TransactionDataMapper(provider.GetService<IPostgresConnectionProvider>() ??
                                      throw new InvalidOperationException()));

        collection.AddScoped<IUserRepository, UserRepository>(provider =>
        {
            IUserDataMapper userDataMapper = provider.GetRequiredService<IUserDataMapper>();
            return new UserRepository(userDataMapper);
        });

        collection.AddScoped<IAdminRepository, AdminRepository>(provider =>
        {
            IAdminDataMapper adminDataMapper = provider.GetRequiredService<IAdminDataMapper>();
            return new AdminRepository(adminDataMapper);
        });
        collection.AddScoped<IBankAccountRepository, BankAccountRepository>(provider =>
        {
            IBankAccountDataMapper bankAccountDataMapper = provider.GetRequiredService<IBankAccountDataMapper>();
            return new BankAccountRepository(bankAccountDataMapper);
        });

        collection.AddScoped<ITransactionRepository, TransactionRepository>(provider =>
        {
            ITransactionDataMapper transactionDataMapper = provider.GetRequiredService<ITransactionDataMapper>();
            return new TransactionRepository(transactionDataMapper);
        });
        return collection;
    }
}