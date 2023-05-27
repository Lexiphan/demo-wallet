using System.Runtime.CompilerServices;

namespace Greentube.DemoWallet.Domain;

public static class EnsureExtensions
{
    public static T EnsureGreaterThan<T>(this T value, T comparand,
        [CallerArgumentExpression("value")] string paramName = "")
        where T : IComparable<T>
    {
        return value.CompareTo(comparand) <= 0
            ? throw new ArgumentOutOfRangeException(paramName, $"{paramName} must be greater than {comparand}")
            : value;
    }

    public static T EnsureLessThan<T>(this T value, T comparand,
        [CallerArgumentExpression("value")] string paramName = "")
        where T : IComparable<T>
    {
        return value.CompareTo(comparand) >= 0
            ? throw new ArgumentOutOfRangeException(paramName, $"{paramName} must be less than {comparand}")
            : value;
    }
}
