using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Greentube.DemoWallet.Database;

public static class QueryableExtensions
{
    public static IQueryable<T> WhereIf<T>(this IQueryable<T> q, bool condition, Expression<Func<T, bool>> predicate)
        where T : class =>
        condition ? q.Where(predicate) : q;

    public static IQueryable<T> IncludeIf<T, TProperty>(
        this IQueryable<T> q,
        bool condition,
        Expression<Func<T, TProperty>> navigationPropertyPath)
        where T : class =>
        condition ? q.Include(navigationPropertyPath) : q;
}
