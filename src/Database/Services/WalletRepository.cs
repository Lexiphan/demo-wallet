using Greentube.DemoWallet.Database.Contracts;
using Greentube.DemoWallet.Domain;
using Microsoft.EntityFrameworkCore;

namespace Greentube.DemoWallet.Database.Services;

internal class WalletRepository : IWalletRepository, IDisposable
{
    private WalletDbContext? _context;

    private WalletDbContext Context => _context ?? throw new InvalidOperationException(
        $"{nameof(WalletRepository)} cannot be used outside of {nameof(WalletDatabase)}.{nameof(WalletDatabase.TransactionAsync)}");

    public void Dispose() => _context = null;

    public async ValueTask AddPlayerAsync(Player player, CancellationToken ct = default)
    {
        await Context.Players.AddAsync(player, ct);
    }

    public Task<Player?> GetPlayerAsync(long id, bool includeTransactions, CancellationToken ct = default)
    {
        return Context.Players
            .Where(x => x.Id == id)
            .IncludeIf(includeTransactions, x => x.Transactions)
            .AsSplitQuery()
            .FirstOrDefaultAsync(ct);
    }

    // public IAsyncEnumerable<Transaction> GetPlayerTransactions(long playerId, TransactionQueryOptions options)
    // {
    //     return Context.Transactions
    //         .Where(x => x.PlayerId == playerId)
    //         .Apply(options)
    //         .AsAsyncEnumerable();
    // }

    public IQueryable<Player> Players => Context.Players.AsQueryable();
    public IQueryable<Transaction> Transactions => Context.Transactions.AsQueryable();

    public WalletRepository(WalletDbContext context) => _context = context;
}
