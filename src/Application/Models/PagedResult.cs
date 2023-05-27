using System.ComponentModel.DataAnnotations;

namespace Greentube.DemoWallet.Application.Models;

/// <summary>
/// Represents multiple entities query result
/// </summary>
public class PagedResult<TItem>
{
    /// <summary>
    /// Page number
    /// </summary>
    [Required]
    public int Page { get; init; }

    /// <summary>
    /// Page size
    /// </summary>
    [Required]
    public int PageSize { get; init; }

    /// <summary>
    /// Flag indicating there is more elements after this page
    /// </summary>
    [Required]
    public bool HasMore { get; init; }

    /// <summary>
    /// Items
    /// </summary>
    [Required]
    public IEnumerable<TItem> Items { get; init; } = null!;
}
