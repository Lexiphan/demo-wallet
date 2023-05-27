using System.ComponentModel.DataAnnotations;

namespace Greentube.DemoWallet.Application.Models;

/// <summary>
/// HTTP request's query arguments for paging
/// </summary>
public sealed class PageArgs
{
    /// <summary>
    /// Page number to retrieve indexed from 0. If omitted, the first page (index 0) is assumed.
    /// </summary>
    [Range(0, int.MaxValue / 1000)]
    public int Page { get; init; } = 0;

    /// <summary>
    /// Page size. If omitted, value 100 is assumed. Ranges from 1 to 1000.
    /// </summary>
    [Range(1, 1000)]
    public int PageSize { get; init; } = 100;
}
