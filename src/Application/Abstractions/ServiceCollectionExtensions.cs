using Microsoft.Extensions.DependencyInjection;

namespace Greentube.DemoWallet.Application.Abstractions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds specified handler type to the service collection and makes all its
    /// <see cref="IQueryHandler{TQuery,TResult}"/> and <see cref="ICommandHandler{TCommand,TResult}"/>
    /// interfaces registered too.
    /// </summary>
    /// <param name="services">Service collection.</param>
    /// <param name="lifetime">Lifetime. By default is Scoped.</param>
    /// <typeparam name="THandler">Handler type.</typeparam>
    /// <exception cref="ArgumentException">
    /// Is thrown when <typeparamref name="THandler"/> is not a class
    /// </exception>
    public static IServiceCollection AddHandler<THandler>(
        this IServiceCollection services,
        ServiceLifetime lifetime = ServiceLifetime.Scoped)
        where THandler : class
    {
        var handlerType = typeof(THandler);

        if (!handlerType.IsClass || handlerType.IsAbstract)
        {
            throw new ArgumentException(
                $"Specified handler type {handlerType.FullName} cannot be an interface or abstract class");
        }

        var hasAny = false;

        foreach (var handlerInterfaceType in handlerType.GetInterfaces().Where(IsCqrsInterface))
        {
            hasAny = true;
            services.AddTransient(handlerInterfaceType, sp => sp.GetRequiredService<THandler>());
        }

        if (!hasAny)
        {
            throw new ArgumentException(
                $"Specified handler type {handlerType.FullName} does not implement any of CQRS interfaces");
        }

        services.Add(new(handlerType, handlerType, lifetime));

        return services;
    }

    private static readonly Type[] CqrsInterfaces =
    {
        typeof(IQueryHandler<,>),
        typeof(ICommandHandler<>),
        typeof(ICommandHandler<,>),
    };

    private static bool IsCqrsInterface(Type type) =>
        type.IsConstructedGenericType && CqrsInterfaces.Contains(type.GetGenericTypeDefinition());
}
