using Lab5.Presentation.Scenarios.AdminLogin;
using Lab5.Presentation.Scenarios.Creation;
using Lab5.Presentation.Scenarios.Deposit;
using Lab5.Presentation.Scenarios.Login;
using Lab5.Presentation.Scenarios.TransactionHistory;
using Lab5.Presentation.Scenarios.ViewBalance;
using Lab5.Presentation.Scenarios.WithDraw;
using Microsoft.Extensions.DependencyInjection;

namespace Lab5.Presentation.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPresentationConsole(this IServiceCollection collection)
    {
        collection.AddScoped<ScenarioRunner>();

        collection.AddScoped<IScenarioProvider, LoginScenarioProvider>();
        collection.AddScoped<IScenarioProvider, AdminLoginScenarioProvider>();
        collection.AddScoped<IScenarioProvider, CreateScenarioProvider>();
        collection.AddScoped<IScenarioProvider, ViewBalanceScenarioProvider>();
        collection.AddScoped<IScenarioProvider, DepositScenarioProvider>();
        collection.AddScoped<IScenarioProvider, WithdrawalScenarioProvider>();
        collection.AddScoped<IScenarioProvider, TransactionHistoryScenarioProvider>();

        return collection;
    }
}