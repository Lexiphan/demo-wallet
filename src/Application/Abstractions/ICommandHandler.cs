namespace Greentube.DemoWallet.Application.Abstractions;

/// <summary>
/// Handler for CQRS command with the result
/// </summary>
public interface ICommandHandler<in TCommand, TResult> where TCommand : ICommand<TResult>
{
    ValueTask<TResult> HandleAsync(TCommand command, CancellationToken ct = default);
}

/// <summary>
/// Handler for CQRS command without a result
/// </summary>
public interface ICommandHandler<in TCommand> where TCommand : ICommand
{
    ValueTask HandleAsync(TCommand command, CancellationToken ct = default);
}
