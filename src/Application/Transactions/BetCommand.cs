using Greentube.DemoWallet.Application.Abstractions;

namespace Greentube.DemoWallet.Application.Transactions;

public record BetCommand(long PlayerId, decimal Amount) : ICommand<BetCommandResult>;
