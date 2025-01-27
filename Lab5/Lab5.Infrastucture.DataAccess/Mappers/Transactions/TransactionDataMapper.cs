using Itmo.Dev.Platform.Postgres.Connection;
using Lab5.Domain.Enteties;
using Lab5.Domain.Models;
using Npgsql;

namespace Lab5.Infrastucture.Mappers.Transactions;

public class TransactionDataMapper : ITransactionDataMapper
{
    private readonly IPostgresConnectionProvider _connectionProvider;

    public TransactionDataMapper(IPostgresConnectionProvider postgresConnectionProvider)
    {
        _connectionProvider = postgresConnectionProvider;
    }

    public void Save(Transaction transaction, Guid accountId)
    {
        using NpgsqlConnection connection = _connectionProvider.GetConnectionAsync(default).AsTask().GetAwaiter().GetResult();
        var command = new NpgsqlCommand(
            @"INSERT INTO transactions (transactions_id, account_id, timestamp, amount, currency, description)
                  VALUES (@transactionId, @accountId, @timestamp, @amount, @currency, @description)
                  ON CONFLICT DO NOTHING",
            connection);

        command.Parameters.AddWithValue("@transactionId", transaction.Id);
        command.Parameters.AddWithValue("@accountId", accountId);
        command.Parameters.AddWithValue("@timestamp", transaction.Date);
        command.Parameters.AddWithValue("@amount", transaction.Amount.Amount);
        command.Parameters.AddWithValue("@currency", transaction.Amount.Currency);
        command.Parameters.AddWithValue("@description", transaction.Description);

        command.ExecuteNonQuery();
    }

    public Transaction? GetByTransactionId(Guid transactionId)
    {
        using NpgsqlConnection connection = _connectionProvider.GetConnectionAsync(default).AsTask().GetAwaiter().GetResult();
        connection.Open();

        var command = new NpgsqlCommand(
            @"SELECT transactions_id, account_id, timestamp, amount, currency, description
                  FROM transactions
                  WHERE transactions_id = @transactionId",
            connection);

        command.Parameters.AddWithValue("@transactionId", transactionId);

        using NpgsqlDataReader reader = command.ExecuteReader();
        if (reader.Read())
        {
            return new Transaction(
                reader.GetGuid(0),
                reader.GetDateTime(2),
                new Money(reader.GetDecimal(3), reader.GetString(4)),
                reader.GetString(5));
        }

        return null;
    }
}
