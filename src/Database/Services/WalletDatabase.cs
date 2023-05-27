using Greentube.DemoWallet.Database.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Greentube.DemoWallet.Database.Services;

internal class WalletDatabase : IWalletDatabase
{
    private readonly WalletDbContext _context;
    private readonly ILogger<WalletDatabase> _logger;
    private readonly WalletDatabaseConfiguration _config;

    public Task TransactionAsync(
        Func<IWalletRepository, ValueTask> action,
        CancellationToken ct = default)
    {
        return TransactionAsync(
            async repository =>
            {
                await action(repository);
                return default(object);
            },
            ct);
    }

    public Task<TResult> TransactionAsync<TResult>(
        Func<IWalletRepository, ValueTask<TResult>> action,
        CancellationToken ct = default)
    {
        return _context.Database.CurrentTransaction != null
            ? ExecuteAsync(ct)
            : _config.ExecutionPolicy.ExecuteAsync(ExecuteInTransactionAsync, ct);

        async Task<TResult> ExecuteAsync(CancellationToken innerCt)
        {
            // Repository instance should be accessed only inside the action
            using var repository = new WalletRepository(_context);
            var result = await action(repository);
            await _context.SaveChangesAsync(innerCt);
            return result;
        }

        async Task<TResult> ExecuteInTransactionAsync(CancellationToken innerCt)
        {
            await using var transaction =
                await _context.Database.BeginTransactionAsync(_config.IsolationLevel, innerCt);

            _context.ChangeTracker.Clear(); // to make each execution clean, we need to remove all tracked entries
            try
            {
                var result = await ExecuteAsync(innerCt);
                await transaction.CommitAsync(innerCt);
                return result;
            }
            catch (Exception e)
            {
                _logger.LogDebug(e, "Transaction rolled back due to exception");
                await transaction.RollbackAsync(innerCt);
                throw;
            }
        }
    }

    public async Task MigrateAsync(CancellationToken ct = default)
    {
        var pendingMigrations = (await _context.Database.GetPendingMigrationsAsync(ct)).ToArray();
        if (pendingMigrations.Length == 0)
        {
            return;
        }

        _logger.LogInformation(
            "Found {MigrationCount} pending migrations: {Migrations}",
            pendingMigrations.Length,
            string.Join(", ", pendingMigrations));

        try
        {
            await _context.Database.MigrateAsync(ct);
            _logger.LogInformation("Database migrated successfully");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Database migration failed");
            throw;
        }
    }

    public WalletDatabase(
        WalletDbContext context,
        ILogger<WalletDatabase> logger,
        IOptions<WalletDatabaseConfiguration> config)
    {
        _context = context;
        _logger = logger;
        _config = config.Value;
    }
}
