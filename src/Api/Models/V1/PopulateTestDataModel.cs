using System.ComponentModel.DataAnnotations;

namespace Greentube.DemoWallet.Api.Models.V1;

/// <summary>
/// Defines arguments for test data population
/// </summary>
public class PopulateTestDataModel
{
    /// <summary>
    /// How many players to add to the database
    /// </summary>
    [Required]
    [Range(1, 1000)]
    public int NumberOfPlayers { get; init; }

    /// <summary>
    /// How many transactions each generated player should have
    /// </summary>
    [Required]
    [Range(0, 1000)]
    public int NumberOfTransactions { get; init; }
}
