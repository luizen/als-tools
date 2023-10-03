using AlsTools.Core.ValueObjects;
using AlsTools.Core.ValueObjects.Devices;

namespace AlsTools.Infrastructure.Extractors.DeviceTypes.Plugin;

public class Vst2PluginFormatExtractor : BasePluginFormatExtractor, IPluginFormatExtractor
{

    /// <summary>
    /// VST2 plugins
    ///     MIDI effect     -> Category = 2
    ///     MIDI instrument -> Category = 2 (no difference)
    ///     Audio FX        -> Category = 1
    /// </summary>
    private readonly IDictionary<string, DeviceSort> deviceSortsByCategory = new Dictionary<string, DeviceSort>()
    {
        ["1"] = DeviceSort.AudioEffect,
        ["2"] = DeviceSort.MidiInstrumentOrEffect
    };

    public Vst2PluginFormatExtractor(ILogger<Vst2PluginFormatExtractor> logger) : base(logger, PluginFormat.VST2)
    {
    }

    protected override string PluginNameXpath => @"PluginDesc/VstPluginInfo/PlugName/@Value";

    protected override string PluginPathXpath => @"PluginDesc/VstPluginInfo/Path/@Value";

    protected override DeviceSort GetPluginSort(XPathNavigator pluginDescNode, string pluginName)
    {
        var category = pluginDescNode.SelectSingleNode(@"PluginDesc/VstPluginInfo/Category/@Value")!.Value;

        // Voxengo SPAN, for instance (and I have no idea why), has <Category Value="3" />
        if (!deviceSortsByCategory.ContainsKey(category))
        {
            logger.LogWarning(@"A plugin was found with an unknown Category node value. Plugin name: {@PluginName}; Category: {@Category}; Expected valid categories: {@ValidCategories}.", pluginName, category, deviceSortsByCategory.Keys);
            return DeviceSort.Unknown;
        }

        return deviceSortsByCategory[category];
    }
}
