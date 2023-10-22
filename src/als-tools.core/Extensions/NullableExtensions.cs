namespace AlsTools.Core;

public static class NullableExtensions
{
    public static bool HasValueTrue(this bool? instance)
    {
        return instance.HasValue && instance.Value;
    }

}
