using Greentube.DemoWallet.Application.Abstractions;

namespace Greentube.DemoWallet.Application.Models;

/// <summary>
/// Represents a query of an entity by pages
/// </summary>
public record PagedEntityQuery<TEntity>(PageArgs PageArgs) : IQuery<PagedResult<TEntity>>;
