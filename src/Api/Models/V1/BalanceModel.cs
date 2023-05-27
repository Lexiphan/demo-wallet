using System.ComponentModel.DataAnnotations;

namespace Greentube.DemoWallet.Api.Models.V1;

/// <summary>
/// Specifies the new player's balance after the operation
/// </summary>
public class BalanceModel
{
    /// <summary>
    /// The amount of money on the player's balance after the operation
    /// </summary>
    [Required]
    public decimal Balance { get; init; }
}
