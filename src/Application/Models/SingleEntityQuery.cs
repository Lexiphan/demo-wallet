using Greentube.DemoWallet.Application.Abstractions;
using Greentube.DemoWallet.Domain;

namespace Greentube.DemoWallet.Application.Models;

/// <summary>
/// Represents a query of an entity by its ID
/// </summary>
public record SingleEntityQuery<TEntity>(long Id) : IQuery<TEntity> where TEntity : Entity;
