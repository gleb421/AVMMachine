using Itmo.Dev.Platform.Postgres.Connection;
using Lab5.Domain.Aggregates;
using Lab5.Domain.Models;
using Lab5.Domain.ValueObjects;
using Lab5.Infrastucture.Mappers.BankAccounts;
using Npgsql;

namespace Lab5.Infrastucture.Mappers.Users;

public class UserDataMapper : IUserDataMapper
{
    private readonly IPostgresConnectionProvider _postgresConnectionProvider;

    public UserDataMapper(IPostgresConnectionProvider postgresConnectionProvider)
    {
        _postgresConnectionProvider = postgresConnectionProvider;
    }

    public void Save(User user)
    {
        using NpgsqlConnection connection = _postgresConnectionProvider.GetConnectionAsync(default).AsTask().GetAwaiter().GetResult();
        using NpgsqlTransaction transaction = connection.BeginTransaction();
        try
        {
            var userCommand = new NpgsqlCommand(
                @"INSERT INTO users (user_id, name, account_id)
                      VALUES (@userId, @name, @accountId)
                      ON CONFLICT (user_id) DO UPDATE SET name = @name, account_id = @accountId",
                connection,
                transaction);

            userCommand.Parameters.AddWithValue("@userId", user.UserId);
            userCommand.Parameters.AddWithValue("@name", user.Name.FirstName);
            userCommand.Parameters.AddWithValue("@accountId", user.Account.AccountId);

            userCommand.ExecuteNonQuery();

            var accountMapper = new BankAccountDataMapper(_postgresConnectionProvider);
            accountMapper.Save(user.Account);

            transaction.Commit();
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    public User? GetById(Guid userId)
    {
        using NpgsqlConnection connection = _postgresConnectionProvider.GetConnectionAsync(default).AsTask().GetAwaiter().GetResult();

        var userCommand = new NpgsqlCommand(
            @"SELECT user_id, name, account_id
                  FROM users
                  WHERE user_id = @userId",
            connection);

        userCommand.Parameters.AddWithValue("@userId", userId);

        using NpgsqlDataReader reader = userCommand.ExecuteReader();
        if (!reader.Read())
            return null;

        Guid userIdValue = reader.GetGuid(0);
        string name = reader.GetString(1);
        Guid accountId = reader.GetGuid(2);

        var accountMapper = new BankAccountDataMapper(_postgresConnectionProvider);
        BankAccount? bankAccount = accountMapper.GetByAccountId(accountId);

        if (bankAccount == null)
            throw new InvalidOperationException("Bank account not found for user.");

        return new User(userIdValue, new Name(name), bankAccount);
    }

    public User? FindUserAndAccount(string accountNumber)
    {
        using NpgsqlConnection connection = _postgresConnectionProvider.GetConnectionAsync(default).AsTask().GetAwaiter().GetResult();

        var command = new NpgsqlCommand(
            @"SELECT u.user_id, u.name, b.account_id, b.account_number, b.pin_code, b.balance, b.currency
                  FROM users u
                  INNER JOIN bank_accounts b ON u.account_id = b.account_id
                  WHERE b.account_number = @accountNumber",
            connection);

        command.Parameters.AddWithValue("@accountNumber", accountNumber);

        using NpgsqlDataReader reader = command.ExecuteReader();
        if (!reader.Read())
        {
            return null;
        }

        var account = new BankAccount(
            reader.GetGuid(2),
            reader.GetString(3),
            new PinCode(reader.GetInt32(4)),
            new Money(reader.GetDecimal(5), reader.GetString(6)));

        var user = new User(
            reader.GetGuid(0),
            new Name(reader.GetString(1)),
            account);

        return user;
    }
}
