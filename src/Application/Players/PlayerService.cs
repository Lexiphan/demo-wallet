using AutoMapper;
using Greentube.DemoWallet.Application.Abstractions;
using Greentube.DemoWallet.Application.Errors;
using Greentube.DemoWallet.Application.Models;
using Greentube.DemoWallet.Database.Contracts;
using Greentube.DemoWallet.Domain;
using Microsoft.EntityFrameworkCore;

namespace Greentube.DemoWallet.Application.Players;

internal class PlayerService :
    IQueryHandler<SingleEntityQuery<Player>, Player>,
    IQueryHandler<PagedEntityQuery<Player>, PagedResult<Player>>,
    ICommandHandler<AddPlayerCommand, Player>,
    ICommandHandler<UpdatePlayerCommand, Player>
{
    private readonly IWalletDatabase _walletDatabase;
    private readonly IMapper _mapper;

    public async ValueTask<Player> HandleAsync(SingleEntityQuery<Player> query, CancellationToken ct = default)
    {
        return await _walletDatabase.TransactionAsync(
            async repository => await repository.Players.Apply(query).FirstOrDefaultAsync(ct)
                                ?? throw new EntityNotFoundException(typeof(Player), query.Id),
            ct);
    }

    public async ValueTask<PagedResult<Player>> HandleAsync(PagedEntityQuery<Player> query, CancellationToken ct = default)
    {
        return await _walletDatabase.TransactionAsync(
            async repository =>
                (await repository.Players
                    .OrderBy(x => x.Id)
                    .ApplyPaging(query)
                    .ToListAsync(ct))
                .ToPagedResult(query),
            ct);
    }

    public async ValueTask<Player> HandleAsync(AddPlayerCommand command, CancellationToken ct = default)
    {
        return await _walletDatabase.TransactionAsync(
            async repository =>
            {
                var newPlayer = _mapper.Map<Player>(command);
                await repository.AddPlayerAsync(newPlayer, ct);
                return newPlayer;
            },
            ct);
    }

    public async ValueTask<Player> HandleAsync(UpdatePlayerCommand command, CancellationToken ct = default)
    {
        return await _walletDatabase.TransactionAsync(
            async repository =>
            {
                var player = await repository.GetPlayerAsync(command.Id, false, ct)
                    ?? throw new EntityNotFoundException(typeof(Player), command.Id);
                return _mapper.Map(command, player);
            },
            ct);
    }

    public PlayerService(IWalletDatabase walletDatabase, IMapper mapper)
    {
        _walletDatabase = walletDatabase;
        _mapper = mapper;
    }
}
