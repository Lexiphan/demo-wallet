using Greentube.DemoWallet.Application.Abstractions;
using Greentube.DemoWallet.Application.Players;
using Greentube.DemoWallet.Application.Transactions;
using Microsoft.Extensions.DependencyInjection;

namespace Greentube.DemoWallet.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        return services
            .AddAutoMapper(typeof(ServiceCollectionExtensions))
            .AddHandler<PlayerService>()
            .AddHandler<TransactionService>();
    }
}
