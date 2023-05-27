namespace Greentube.DemoWallet.Application.Models;

/// <summary>
/// Represents a pair of nullable values which is used for filtering purpose
/// </summary>
/// <param name="GreaterOrEqualTo">If defined, filter all entities which have a field with this value or greater</param>
/// <param name="LessThan">If defined, filter all entities which have a field with the value less than this</param>
public sealed record RangeFilter<T>(T? GreaterOrEqualTo, T? LessThan) where T : struct
{
    public static readonly RangeFilter<T> Empty = new(null, null);
}
