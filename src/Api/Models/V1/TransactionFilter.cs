using Greentube.DemoWallet.Application.Models;
using Greentube.DemoWallet.Domain;

namespace Greentube.DemoWallet.Api.Models.V1;

/// <summary>
/// Specifies filter criteria for transactions
/// </summary>
public class TransactionFilter
{
    /// <summary>
    /// Specifies the range of transaction's created date to return
    /// </summary>
    public RangeFilter<DateTime>? CreatedOn { get; init; }

    /// <summary>
    /// Specifies the type of transactions to return
    /// </summary>
    public TransactionType? Type { get; init; }

    /// <summary>
    /// Specifies sort direction for transactions
    /// </summary>
    public SortDirection? SortDirection { get; init; }
}
