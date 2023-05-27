using System.ComponentModel.DataAnnotations;

namespace Greentube.DemoWallet.Api.Models.V1;

/// <summary>
/// Specifies the amount of money for the operation
/// </summary>
public class PlayerAmountModel
{
    /// <summary>
    /// The ID of the player which the operation changes the balance of
    /// </summary>
    [Required]
    public long PlayerId { get; init; }

    /// <summary>
    /// The amount of money for the operation. Must be positive
    /// </summary>
    [Required]
    [Range(0.01, 1_000_000)]
    public decimal Amount { get; init; }
}
