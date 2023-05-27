namespace Greentube.DemoWallet.Database.Contracts;

public interface IWalletDatabase
{
    Task TransactionAsync(
        Func<IWalletRepository, ValueTask> action,
        CancellationToken ct = default);

    Task<TResult> TransactionAsync<TResult>(
        Func<IWalletRepository, ValueTask<TResult>> action,
        CancellationToken ct = default);

    Task MigrateAsync(CancellationToken ct = default);
}
