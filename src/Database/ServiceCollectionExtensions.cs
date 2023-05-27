using System.Reflection;
using Greentube.DemoWallet.Database.Contracts;
using Greentube.DemoWallet.Database.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Npgsql;
using Polly;

namespace Greentube.DemoWallet.Database;

public static class ServiceCollectionExtensions
{
    private const string ConfigSection = "WalletDatabase";

    public static IServiceCollection AddWalletDatabase(this IServiceCollection services, string configSection = ConfigSection)
    {
        services.AddOptions<WalletDatabaseConfiguration>()
            .BindConfiguration(configSection)
            .ValidateDataAnnotations()
            .ValidateOnStart()
            .Configure(
                options =>
                    options.ExecutionPolicy = Policy
                        .Handle<PostgresException>(exception => exception.IsTransient)
                        .RetryAsync(options.RetryAttemptsOnTransientError));

        return services
            .AddDbContext<WalletDbContext>((sp, options) =>
            {
                var config = sp.GetRequiredService<IOptions<WalletDatabaseConfiguration>>().Value;
                var connectionStringBuilder = new NpgsqlConnectionStringBuilder
                {
                    Host = config.Host,
                    Database = config.Database,
                    Username = config.User,
                    Password = config.Password,
                    ApplicationName = Assembly.GetEntryAssembly()!.GetName().Name,
                };
                options.UseNpgsql(connectionStringBuilder.ConnectionString);
            })
            .AddScoped<IWalletDatabase, WalletDatabase>();
    }
}
