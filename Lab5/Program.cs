using FluentMigrator.Runner;
using Lab5.Application.Extensions;
using Lab5.Infrastucture.Extensions;
using Lab5.Presentation;
using Lab5.Presentation.Extensions;
using Microsoft.Extensions.DependencyInjection;

var collection = new ServiceCollection();

collection
    .AddApplication()
    .AddInfrastructureDataAccess(configuration =>
    {
        configuration.Host = "localhost";
        configuration.Port = 5433;
        configuration.Username = "postgres";
        configuration.Password = "postgres";
        configuration.Database = "postgres";
        configuration.SslMode = "Prefer";
    })
    .AddPresentationConsole();

ServiceProvider provider = collection.BuildServiceProvider();
using IServiceScope scope = provider.CreateScope();
IMigrationRunner runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
runner.MigrateUp();
scope.UseInfrastructureDataAccess();

ScenarioRunner scenarioRunner = scope.ServiceProvider
    .GetRequiredService<ScenarioRunner>();
while (true)
{
    scenarioRunner.Run();
}