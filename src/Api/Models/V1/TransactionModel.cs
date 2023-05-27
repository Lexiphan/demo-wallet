using System.ComponentModel.DataAnnotations;
using Greentube.DemoWallet.Domain;

namespace Greentube.DemoWallet.Api.Models.V1;

/// <summary>
/// Describes a transaction
/// </summary>
public class TransactionModel
{
    /// <summary>
    /// The date the player was created
    /// </summary>
    [Required]
    public DateTime CreatedOn { get; init; }

    /// <summary>
    /// The type of the transaction
    /// </summary>
    [Required]
    public TransactionType Type { get; init; }

    /// <summary>
    /// The amount of money added to or subtracted from the player's balance.
    /// For TopUp and Win transactions it is positive, for Bet transactions it is negative.
    /// </summary>
    [Required]
    public decimal Amount { get; init; }
}
