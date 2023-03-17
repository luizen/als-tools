namespace AlsTools.Core.Extensions;

public static class EnumerableExtensions
{
    public static bool HasValues<T>(this IEnumerable<T>? instance)
    {
        return instance != null && instance.Any();
    }
}