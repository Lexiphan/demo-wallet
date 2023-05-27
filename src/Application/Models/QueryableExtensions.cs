using System.Linq.Expressions;
using Greentube.DemoWallet.Domain;
using Microsoft.EntityFrameworkCore;

namespace Greentube.DemoWallet.Application.Models;

public static class QueryableExtensions
{
    public static IQueryable<TEntity> Apply<TEntity>(
        this IQueryable<TEntity> entityQuery,
        SingleEntityQuery<TEntity> query)
        where TEntity : Entity
    {
        return entityQuery.Where(x => x.Id == query.Id);
    }

    public static IQueryable<TEntity> ApplyPaging<TEntity>(
        this IQueryable<TEntity> entityQuery,
        PagedEntityQuery<TEntity> pagedQuery)
        where TEntity : Entity
    {
        var startIndex = pagedQuery.PageArgs.Page * pagedQuery.PageArgs.PageSize;

        if (startIndex > 0)
        {
            entityQuery = entityQuery.Skip(startIndex);
        }

        return entityQuery
            .Take(pagedQuery.PageArgs.PageSize + 1) // +1 because we want to know if there are more elements
            .AsNoTracking();
    }

    public static IOrderedQueryable<TEntity> OrderByDirection<TEntity, TProperty>(
        this IQueryable<TEntity> entityQuery,
        SortDirection sortDirection,
        Expression<Func<TEntity, TProperty>> propertySelector)
    {
        return sortDirection switch
        {
            SortDirection.Ascending => entityQuery.OrderBy(propertySelector),
            SortDirection.Descending => entityQuery.OrderByDescending(propertySelector),
            _ => throw new ArgumentOutOfRangeException(nameof(sortDirection), sortDirection, null)
        };
    }

    public static IOrderedQueryable<TEntity> ThenByDirection<TEntity, TProperty>(
        this IOrderedQueryable<TEntity> entityQuery,
        SortDirection sortDirection,
        Expression<Func<TEntity, TProperty>> propertySelector)
    {
        return sortDirection switch
        {
            SortDirection.Ascending => entityQuery.ThenBy(propertySelector),
            SortDirection.Descending => entityQuery.ThenByDescending(propertySelector),
            _ => throw new ArgumentOutOfRangeException(nameof(sortDirection), sortDirection, null)
        };
    }

    public static IQueryable<TEntity> ApplyRange<TEntity, TProperty>(
        this IQueryable<TEntity> entityQuery,
        RangeFilter<TProperty> range,
        Expression<Func<TEntity, TProperty>> propertySelector)
        where TProperty : struct
    {
        if (range.GreaterOrEqualTo.HasValue)
        {
            entityQuery = entityQuery.Where(
                BuildPredicate(Expression.GreaterThanOrEqual, propertySelector, () => range.GreaterOrEqualTo.Value));
        }

        if (range.LessThan.HasValue)
        {
            entityQuery = entityQuery.Where(
                BuildPredicate(Expression.LessThan, propertySelector, () => range.LessThan.Value));
        }

        return entityQuery;
    }

    private static Expression<Func<TEntity, bool>> BuildPredicate<TEntity, TProperty>(
        Func<Expression, Expression, BinaryExpression> operation,
        Expression<Func<TEntity, TProperty>> left,
        Expression<Func<TProperty>> right)
    {
        return Expression.Lambda<Func<TEntity, bool>>(operation(left.Body, right.Body), left.Parameters);
    }
}
