using Itmo.Dev.Platform.Postgres.Connection;
using Lab5.Domain.Enteties;
using Npgsql;

namespace Lab5.Infrastucture.Mappers.Admins;

public class AdminDataMapper : IAdminDataMapper
{
    private readonly IPostgresConnectionProvider _connectionProvider;

    public AdminDataMapper(IPostgresConnectionProvider connectionProvider)
    {
        _connectionProvider = connectionProvider;
    }

    public Admin? GetAdminById(Guid adminId)
    {
        using NpgsqlConnection connection = _connectionProvider
            .GetConnectionAsync(default)
            .AsTask()
            .GetAwaiter()
            .GetResult();

        var command = new NpgsqlCommand(
            @"SELECT admin_id, system_password 
                  FROM admins 
                  WHERE admin_id = @adminId",
            connection);

        command.Parameters.AddWithValue("@adminId", adminId);

        using NpgsqlDataReader reader = command.ExecuteReader();
        if (reader.Read())
        {
            return new Admin(
                reader.GetGuid(0),
                reader.GetString(1));
        }

        return null;
    }

    public void Save(Admin admin)
    {
        using NpgsqlConnection connection = _connectionProvider
            .GetConnectionAsync(default)
            .AsTask()
            .GetAwaiter()
            .GetResult();

        var command = new NpgsqlCommand(
            @"INSERT INTO admins (admin_id, system_password)
                  VALUES (@adminId, @systemPassword)
                  ON CONFLICT (admin_id) DO UPDATE SET system_password = @systemPassword",
            connection);

        command.Parameters.AddWithValue("@adminId", admin.AdminId);
        command.Parameters.AddWithValue("@systemPassword", admin.SystemPassword);

        command.ExecuteNonQuery();
    }
}