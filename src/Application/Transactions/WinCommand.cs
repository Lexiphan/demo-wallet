using Greentube.DemoWallet.Application.Abstractions;

namespace Greentube.DemoWallet.Application.Transactions;

public record WinCommand(long PlayerId, decimal Amount) : ICommand<decimal>;
