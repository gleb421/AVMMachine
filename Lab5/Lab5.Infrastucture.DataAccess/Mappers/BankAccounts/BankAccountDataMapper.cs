using Itmo.Dev.Platform.Postgres.Connection;
using Lab5.Domain.Aggregates;
using Lab5.Domain.Enteties;
using Lab5.Domain.Models;
using Lab5.Domain.ValueObjects;
using Npgsql;

namespace Lab5.Infrastucture.Mappers.BankAccounts;

public class BankAccountDataMapper : IBankAccountDataMapper
{
    private readonly IPostgresConnectionProvider _postgresConnectionProvider;

    public BankAccountDataMapper(IPostgresConnectionProvider postgresConnectionProvider)
    {
        _postgresConnectionProvider = postgresConnectionProvider;
    }

    public void Save(BankAccount account)
    {
        using NpgsqlConnection connection = _postgresConnectionProvider
            .GetConnectionAsync(default)
            .AsTask()
            .GetAwaiter()
            .GetResult();

        using NpgsqlTransaction transaction = connection.BeginTransaction();

        try
        {
            var command = new NpgsqlCommand(
                @"INSERT INTO bank_accounts (account_id, account_number, pin_code, balance, currency)
              VALUES (@accountId, @accountNumber, @pinCode, @balance, @currency)
              ON CONFLICT (account_id) DO UPDATE SET balance = @balance, currency = @currency",
                connection,
                transaction);

            command.Parameters.AddWithValue("@accountId", account.AccountId);
            command.Parameters.AddWithValue("@accountNumber", account.AccountNumber);
            command.Parameters.AddWithValue("@pinCode", account.PinCode.Pin);
            command.Parameters.AddWithValue("@balance", account.Balance.Amount);
            command.Parameters.AddWithValue("@currency", account.Balance.Currency);

            command.ExecuteNonQuery();

            foreach (Transaction transactionn in account.Transactions)
            {
                var transactionCommand = new NpgsqlCommand(
                    @"INSERT INTO transactions (transactions_id, account_id, timestamp, amount, currency, description)
                  VALUES (@transactionId, @accountId, @timestamp, @amount, @currency, @description)
                  ON CONFLICT DO NOTHING",
                    connection,
                    transaction);

                transactionCommand.Parameters.AddWithValue("@transactionId", transactionn.Id);
                transactionCommand.Parameters.AddWithValue("@accountId", account.AccountId);
                transactionCommand.Parameters.AddWithValue("@timestamp", transactionn.Date);
                transactionCommand.Parameters.AddWithValue("@amount", transactionn.Amount.Amount);
                transactionCommand.Parameters.AddWithValue("@currency", transactionn.Amount.Currency);
                transactionCommand.Parameters.AddWithValue("@description", transactionn.Description);

                transactionCommand.ExecuteNonQuery();
            }

            transaction.Commit();
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            Console.WriteLine($"Error: {ex.Message}");
            throw;
        }
    }

    public void Update(BankAccount account, Money amount, string transactionDescription)
    {
        using NpgsqlConnection connection = _postgresConnectionProvider
            .GetConnectionAsync(default)
            .AsTask()
            .GetAwaiter()
            .GetResult();

        using NpgsqlTransaction transaction = connection.BeginTransaction();

        try
        {
            var checkAccountCommand = new NpgsqlCommand(
                @"SELECT 1 FROM bank_accounts WHERE account_id = @accountId",
                connection,
                transaction);

            checkAccountCommand.Parameters.AddWithValue("@accountId", account.AccountId);

            bool accountExists = checkAccountCommand.ExecuteScalar() != null;

            if (!accountExists)
            {
                throw new InvalidOperationException("Account not found.");
            }

            var updateCommand = new NpgsqlCommand(
                @"UPDATE bank_accounts
              SET balance = @amount
              WHERE account_id = @accountId",
                connection,
                transaction);

            updateCommand.Parameters.AddWithValue("@accountId", account.AccountId);
            updateCommand.Parameters.AddWithValue("@amount", amount.Amount);

            int rowsAffected = updateCommand.ExecuteNonQuery();

            if (rowsAffected == 0)
            {
                throw new InvalidOperationException("Failed to update the account balance.");
            }

            var transactionCommand = new NpgsqlCommand(
                @"INSERT INTO transactions (transactions_id, account_id, timestamp, amount, currency, description)
              VALUES (@transactionId, @accountId, @timestamp, @amount, @currency, @description)",
                connection,
                transaction);

            var transactionId = Guid.NewGuid();
            transactionCommand.Parameters.AddWithValue("@transactionId", transactionId);
            transactionCommand.Parameters.AddWithValue("@accountId", account.AccountId);
            transactionCommand.Parameters.AddWithValue("@timestamp", DateTime.UtcNow);
            transactionCommand.Parameters.AddWithValue("@amount", amount.Amount);
            transactionCommand.Parameters.AddWithValue("@currency", amount.Currency);
            transactionCommand.Parameters.AddWithValue("@description", transactionDescription);

            transactionCommand.ExecuteNonQuery();

            transaction.Commit();
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            Console.WriteLine($"Error: {ex.Message}");
            throw;
        }
    }

    public BankAccount? GetByAccountId(Guid accountId)
    {
        using NpgsqlConnection connection = _postgresConnectionProvider
            .GetConnectionAsync(default)
            .AsTask()
            .GetAwaiter()
            .GetResult();

        var command = new NpgsqlCommand(
            @"SELECT account_id, account_number, pin_code, balance, currency
                  FROM bank_accounts
                  WHERE account_id = @accountId",
            connection);

        command.Parameters.AddWithValue("@accountId", accountId);

        using NpgsqlDataReader reader = command.ExecuteReader();
        if (!reader.Read())
            return null;

        var bankAccount = new BankAccount(
            reader.GetGuid(0),
            reader.GetString(1),
            new PinCode(reader.GetInt32(2)),
            new Money(reader.GetDecimal(3), reader.GetString(4)));

        IEnumerable<Transaction> transactions = LoadTransactions(connection, accountId);
        bankAccount.LoadTransactions(transactions);

        return bankAccount;
    }

    private List<Transaction> LoadTransactions(NpgsqlConnection connection, Guid accountId)
    {
        var command = new NpgsqlCommand(
            @"SELECT transactions_id, timestamp, amount, currency, description 
                  FROM transactions
                  WHERE account_id = @accountId",
            connection);

        command.Parameters.AddWithValue("@accountId", accountId);

        using NpgsqlDataReader reader = command.ExecuteReader();
        var transactions = new List<Transaction>();

        while (reader.Read())
        {
            transactions.Add(new Transaction(
                reader.GetGuid(0),
                reader.GetDateTime(1),
                new Money(reader.GetDecimal(2), reader.GetString(3)),
                reader.GetString(4)));
        }

        return transactions;
    }
}