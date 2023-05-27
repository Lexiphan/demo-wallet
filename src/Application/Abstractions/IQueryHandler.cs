namespace Greentube.DemoWallet.Application.Abstractions;

/// <summary>
/// Handler for CQRS query
/// </summary>
public interface IQueryHandler<in TQuery, TResult> where TQuery : IQuery<TResult>
{
    ValueTask<TResult> HandleAsync(TQuery query, CancellationToken ct = default);
}
