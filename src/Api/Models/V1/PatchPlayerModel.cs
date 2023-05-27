using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Greentube.DemoWallet.Api.Models.V1;

/// <summary>
/// Data necessary to add new player
/// </summary>
public class PatchPlayerModel
{
    /// <summary>
    /// ID of the player to patch
    /// </summary>
    [JsonIgnore]
    public long Id { get; set; }

    /// <summary>
    /// Player name.
    /// If omitted, player's name is not changed
    /// </summary>
    [MinLength(1)]
    public string? Name { get; init; }

    /// <summary>
    /// The limit which prohibits making a bet if the player's balance falls below it.
    /// Can be positive as well as negative.
    /// If omitted, player's BalanceLimit is not changed
    /// </summary>
    [Range(-1000, 1000)]
    public decimal? BalanceLimit { get; init; }
}
