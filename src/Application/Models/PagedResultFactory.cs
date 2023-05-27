namespace Greentube.DemoWallet.Application.Models;

public static class PagedResultFactory
{
    /// <summary>
    /// Converts the result of paged query execution to the result page model.
    /// </summary>
    /// <param name="queryResult">
    /// This must be the result of the enumeration created by the call to
    /// <see cref="QueryableExtensions.ApplyPaging{TEntity}"/>
    /// </param>
    /// <param name="pagedQuery">
    /// The query which was used in <see cref="QueryableExtensions.ApplyPaging{TEntity}"/>
    /// </param>
    public static PagedResult<TItem> ToPagedResult<TItem>(
        this IReadOnlyCollection<TItem> queryResult,
        PagedEntityQuery<TItem> pagedQuery)
    {
        var hasMore = queryResult.Count > pagedQuery.PageArgs.PageSize;

        return new()
        {
            Page = pagedQuery.PageArgs.Page,
            PageSize = pagedQuery.PageArgs.PageSize,
            HasMore = hasMore,
            Items = hasMore ? queryResult.Take(pagedQuery.PageArgs.PageSize) : queryResult,
        };
    }
}
