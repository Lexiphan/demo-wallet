using Greentube.DemoWallet.Application.Abstractions;
using Greentube.DemoWallet.Application.Errors;
using Greentube.DemoWallet.Application.Models;
using Greentube.DemoWallet.Database;
using Greentube.DemoWallet.Database.Contracts;
using Greentube.DemoWallet.Domain;
using Microsoft.EntityFrameworkCore;

namespace Greentube.DemoWallet.Application.Transactions;

internal class TransactionService :
    IQueryHandler<GetPlayerTransactionsQuery, PagedResult<Transaction>>,
    ICommandHandler<TopUpCommand, decimal>,
    ICommandHandler<BetCommand, BetCommandResult>,
    ICommandHandler<WinCommand, decimal>
{
    private readonly IWalletDatabase _walletDatabase;

    public async ValueTask<PagedResult<Transaction>> HandleAsync(GetPlayerTransactionsQuery query, CancellationToken ct = default)
    {
        return await _walletDatabase.TransactionAsync(
            async repository =>
                (await repository.Transactions
                    .Where(x => x.PlayerId == query.PlayerId)
                    .ApplyRange(query.CreatedOn, x => x.CreatedOn)
                    .WhereIf(query.Type.HasValue, x => x.Type == query.Type)
                    .OrderByDirection(query.SortDirection, x => x.CreatedOn)
                    .ThenByDirection(query.SortDirection, x => x.Id)
                    .ApplyPaging(query)
                    .ToListAsync(ct))
                .ToPagedResult(query),
            ct);
    }

    public async ValueTask<decimal> HandleAsync(TopUpCommand command, CancellationToken ct = default)
    {
        return await _walletDatabase.TransactionAsync(
            async repository =>
            {
                var player = await repository.GetPlayerAsync(command.PlayerId, true, ct)
                             ?? throw new EntityNotFoundException(typeof(Player), command.PlayerId);
                player.TopUp(command.Amount);
                return player.Balance;
            },
            ct);
    }

    public async ValueTask<BetCommandResult> HandleAsync(BetCommand command, CancellationToken ct = default)
    {
        return await _walletDatabase.TransactionAsync(
            async repository =>
            {
                var player = await repository.GetPlayerAsync(command.PlayerId, true, ct)
                             ?? throw new EntityNotFoundException(typeof(Player), command.PlayerId);
                var succeeded = player.Bet(command.Amount);
                return new BetCommandResult(player.Balance, succeeded);
            },
            ct);
    }

    public async ValueTask<decimal> HandleAsync(WinCommand command, CancellationToken ct = default)
    {
        return await _walletDatabase.TransactionAsync(
            async repository =>
            {
                var player = await repository.GetPlayerAsync(command.PlayerId, true, ct)
                             ?? throw new EntityNotFoundException(typeof(Player), command.PlayerId);
                player.Win(command.Amount);
                return player.Balance;
            },
            ct);
    }

    public TransactionService(IWalletDatabase walletDatabase)
    {
        _walletDatabase = walletDatabase;
    }
}
