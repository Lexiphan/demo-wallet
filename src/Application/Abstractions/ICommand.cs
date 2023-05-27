namespace Greentube.DemoWallet.Application.Abstractions;

/// <summary>
/// Represents CQRS command without a result
/// </summary>
public interface ICommand
{
}

/// <summary>
/// Represents CQRS command with the result
/// </summary>
public interface ICommand<TResult>
{
}
