using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace AlsTools.Core.Models.Devices;


public class PluginDevice : BaseDevice
{
    public PluginDevice(DeviceSort sort, PluginFormat format, string path = "") : base(sort, DeviceType.Plugin)
    {
        Format = format;
        Path = path;
    }

    public PluginFormat Format { get; }

    public string Path { get; set; }
}

public partial class PluginDeviceEqualityComparer : IEqualityComparer<PluginDevice>
{
    private readonly bool exactNameMatch;

    public PluginDeviceEqualityComparer(bool exactNameMatch)
    {
        this.exactNameMatch = exactNameMatch;
    }

    public bool Equals(PluginDevice? x, PluginDevice? y)
    {
        if (ReferenceEquals(x, y))
            return true;

        if (x is null || y is null)
            return false;

        var nameX = Preprocess(x.Name);
        var nameY = Preprocess(y.Name);

        bool nameMatch;

        /*
            😔
            It seems there is no easy way to compare the similarities between a plugin name and its file name.
            Using fuzzy comparison seemed to be the answer, but there are exceptions and lots of corner cases.
            Let's try to do our best, but keep it simple...
        */

        // var fuzzyRatio = Fuzz.Ratio(nameX, nameY);
        // var partialRatio = Fuzz.PartialRatio(nameX, nameY);
        // var tokenSetRatio = Fuzz.TokenSetRatio(nameX, nameY);
        // var partialTokenSetRatio = Fuzz.PartialTokenSetRatio(nameX, nameY);
        // var tokenAbbreviationRatio = Fuzz.TokenAbbreviationRatio(nameX, nameY);
        // var partialTokenAbbreviationRatio = Fuzz.PartialTokenAbbreviationRatio(nameX, nameY);
        // var weightedRatio = Fuzz.WeightedRatio(nameX, nameY);

        nameMatch = nameX == nameY;
        // if (exactNameMatch)
        //     nameMatch = nameX == nameY;
        // else
        //     nameMatch = fuzzyRatio > 95;


        // Console.WriteLine($"nameX: {nameX} / nameY: {nameY} / FuzzyRatio: {fuzzyRatio}");
        // Console.WriteLine($"nameX: {nameX} / nameY: {nameY} / PartialRatio: {partialRatio}");
        // Console.WriteLine($"nameX: {nameX} / nameY: {nameY} / TokenSetRatio: {tokenSetRatio}");
        // Console.WriteLine($"nameX: {nameX} / nameY: {nameY} / PartialTokenSetRatio: {partialTokenSetRatio}");
        // Console.WriteLine($"nameX: {nameX} / nameY: {nameY} / TokenAbbreviationRatio: {tokenAbbreviationRatio}");
        // Console.WriteLine($"nameX: {nameX} / nameY: {nameY} / PartialTokenAbbreviationRatio: {partialTokenAbbreviationRatio}");
        // Console.WriteLine($"nameX: {nameX} / nameY: {nameY} / WeightedRatio: {weightedRatio}");

        // if (fuzzyRatio > 85)// && fuzzyRatio < 100)
        //     // Console.WriteLine($"x.Name: {x.Name} / y.Name: {y.Name} / FuzzyRatio: {fuzzyRatio}");
        //     Console.WriteLine($"nameX: {nameX} / nameY: {nameY} / FuzzyRatio: {fuzzyRatio}");

        return x.Format == y.Format && nameMatch;
    }

    public int GetHashCode([DisallowNull] PluginDevice obj)
    {
        int hash = 17;
        hash = hash * 31 + obj.Format.GetHashCode();

        if (obj.Name != null)
        {
            hash = hash * 31 + StringComparer.Ordinal.GetHashCode(obj.Name);
        }

        return hash;
    }

    private string Preprocess(string word)
    {
        // Remove spaces and special characters, and convert to lowercase
        return IsSpecialCharacterRegex().Replace(word, "").ToLower();
    }

    [GeneratedRegex("[^a-zA-Z0-9]", RegexOptions.Compiled, 100)]
    private static partial Regex IsSpecialCharacterRegex();
}

// public class PluginDeviceNameFuzzyEqualityComparer : IEqualityComparer<string>
// {
//     public bool Equals(string? x, string? y)
//     {
//         if (ReferenceEquals(x, y))
//             return true;

//         if (x is null || y is null)
//             return false;

//         var fuzzyRatio = Fuzz.Ratio(x, y);

//         return fuzzyRatio > 70;
//     }

//     public int GetHashCode([DisallowNull] string obj)
//     {
//         return obj.GetHashCode();
//     }
// }

public class PluginDevicePathEqualityComparer : IEqualityComparer<PluginDevice>
{
    public bool Equals(PluginDevice? x, PluginDevice? y)
    {
        if (ReferenceEquals(x, y))
            return true;

        if (x is null || y is null)
            return false;

        return x.Path == y.Path;
    }

    public int GetHashCode([DisallowNull] PluginDevice obj)
    {
        return obj.Path.GetHashCode();
    }
}
