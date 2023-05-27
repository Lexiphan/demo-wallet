using Greentube.DemoWallet.Domain;

namespace Greentube.DemoWallet.Database.Contracts;

public interface IWalletRepository
{
    ValueTask AddPlayerAsync(Player player, CancellationToken ct = default);
    Task<Player?> GetPlayerAsync(long id, bool includeTransactions, CancellationToken ct = default);

    IQueryable<Player> Players { get; }
    IQueryable<Transaction> Transactions { get; }
}
