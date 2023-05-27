using Greentube.DemoWallet.Application.Abstractions;
using Greentube.DemoWallet.Domain;

namespace Greentube.DemoWallet.Application.Players;

public record UpdatePlayerCommand(long Id, string? Name, decimal? BalanceLimit) : ICommand<Player>;
