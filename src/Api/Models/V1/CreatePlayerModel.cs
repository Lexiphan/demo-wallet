using System.ComponentModel.DataAnnotations;

namespace Greentube.DemoWallet.Api.Models.V1;

/// <summary>
/// Data necessary to add new player
/// </summary>
public class CreatePlayerModel
{
    /// <summary>
    /// Player name
    /// </summary>
    [Required]
    public string Name { get; init; } = null!;

    /// <summary>
    /// The limit which prohibits making a bet if the player's balance falls below it.
    /// Can be positive as well as negative
    /// </summary>
    [Required]
    [Range(-1000, 1000)]
    public decimal BalanceLimit { get; init; }
}
