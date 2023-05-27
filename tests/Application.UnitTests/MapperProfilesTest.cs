using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace Greentube.DemoWallet.Application.UnitTests;

public class MapperProfilesTest : IDisposable
{
    private readonly ServiceProvider _serviceProvider;

    public MapperProfilesTest()
    {
        _serviceProvider = new ServiceCollection()
            .AddAutoMapper(typeof(ServiceCollectionExtensions))
            .BuildServiceProvider();
    }

    public void Dispose()
    {
        _serviceProvider.Dispose();
    }

    [Fact]
    public void MapperConfigurationIsValid()
    {
        _serviceProvider.GetRequiredService<IConfigurationProvider>().AssertConfigurationIsValid();
    }
}
