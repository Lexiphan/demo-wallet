using Greentube.DemoWallet.Application.Models;
using Greentube.DemoWallet.Domain;

namespace Greentube.DemoWallet.Application.Transactions;

public record GetPlayerTransactionsQuery(
        PageArgs PageArgs,
        long PlayerId,
        RangeFilter<DateTime> CreatedOn,
        TransactionType? Type,
        SortDirection SortDirection)
    : PagedEntityQuery<Transaction>(PageArgs);
