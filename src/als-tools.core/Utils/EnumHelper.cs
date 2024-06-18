using System.Text.RegularExpressions;

namespace AlsTools.Core.Extensions;

public static class EnumHelper
{
    public static string[] GetCamelCaseNames(Type enumType)
    {
        return Enum.GetNames(enumType).Select(x => x.SplitCamelCase()).ToArray();
    }

    private static string SplitCamelCase(this string instance)
    {
        return Regex.Replace(instance, "([A-Z])", " $1", RegexOptions.Compiled).Trim();
    }
}