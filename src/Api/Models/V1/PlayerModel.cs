using System.ComponentModel.DataAnnotations;

namespace Greentube.DemoWallet.Api.Models.V1;

/// <summary>
/// Describes a player
/// </summary>
public class PlayerModel : CreatePlayerModel
{
    /// <summary>
    /// Player ID
    /// </summary>
    [Required]
    public long Id { get; init; }

    /// <summary>
    /// The current balance of the player.
    /// Can be positive as well as negative if BalanceLimit allows that
    /// </summary>
    [Required]
    public decimal Balance { get; init; }

    /// <summary>
    /// The date the player was created
    /// </summary>
    [Required]
    public DateTime CreatedOn { get; init; }
}
