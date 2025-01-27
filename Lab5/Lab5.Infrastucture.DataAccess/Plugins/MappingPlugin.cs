using Itmo.Dev.Platform.Postgres.Plugins;
using Lab5.Infrastucture.Mappers.Users;
using Npgsql;

namespace Lab5.Infrastucture.Plugins;

public class MappingPlugin : IDataSourcePlugin
{
    public void Configure(NpgsqlDataSourceBuilder builder)
    {
        builder.MapEnum<UserRole>();
    }
}