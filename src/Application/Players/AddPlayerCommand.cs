using Greentube.DemoWallet.Application.Abstractions;
using Greentube.DemoWallet.Domain;

namespace Greentube.DemoWallet.Application.Players;

public record AddPlayerCommand(string Name, decimal BalanceLimit) : ICommand<Player>;
