using System.Collections.ObjectModel;
using AlsTools.Core.ValueObjects;

namespace AlsTools.Infrastructure.XmlNodeNames;

public static class PluginFormatNodeName
{
    public const string VST2 = "VSTPLUGININFO";

    public const string VST3 = "VST3PLUGININFO";

    public const string AU = "AUPLUGINDEVICE";

    private static readonly IReadOnlyDictionary<PluginFormat, string> nodeNamesByPluginFormat = new ReadOnlyDictionary<PluginFormat, string>
    (
        new Dictionary<PluginFormat, string>()
        {
            [PluginFormat.AU] = AU,
            [PluginFormat.VST2] = VST2,
            [PluginFormat.VST3] = VST3
        }
    );

    private static readonly IReadOnlyDictionary<string, PluginFormat> pluginFormatsByNodeNames = new ReadOnlyDictionary<string, PluginFormat>
    (
        new Dictionary<string, PluginFormat>()
        {
            [AU] = PluginFormat.AU,
            [VST2] = PluginFormat.VST2,
            [VST3] = PluginFormat.VST3
        }
    );

    public static string GetNodeNameFromPluginFormat(PluginFormat format)
    {
        return nodeNamesByPluginFormat[format];
    }

    public static PluginFormat GetPluginFormatFromNodeName(string nodeName)
    {
        return pluginFormatsByNodeNames[nodeName];
    }
}