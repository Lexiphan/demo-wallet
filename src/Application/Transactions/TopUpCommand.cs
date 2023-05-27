using Greentube.DemoWallet.Application.Abstractions;

namespace Greentube.DemoWallet.Application.Transactions;

public record TopUpCommand(long PlayerId, decimal Amount) : ICommand<decimal>;
