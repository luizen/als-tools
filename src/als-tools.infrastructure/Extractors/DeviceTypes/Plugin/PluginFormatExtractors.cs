using AlsTools.Core.ValueObjects;
using AlsTools.Core.ValueObjects.Devices;

namespace AlsTools.Infrastructure.Extractors.DeviceTypes.Plugin;

public interface IPluginFormatExtractor : IDeviceExtractor
{
}

public class AuPluginFormatExtractor : BasePluginFormatExtractor, IPluginFormatExtractor
{
    public AuPluginFormatExtractor(ILogger<AuPluginFormatExtractor> logger) : base(logger, PluginFormat.AU)
    {
    }

    protected override string PluginNameXpath => @"PluginDesc/AuPluginInfo/Name/@Value";

    protected override DeviceSort GetPluginSort(XPathNavigator pluginDescNode, string pluginName)
    {
        return DeviceSort.Unknown;
    }
}

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

public class Vst3PluginFormatExtractor : BasePluginFormatExtractor, IPluginFormatExtractor
{
    /// <summary>
    /// VST3 plugins
    ///     MIDI effect     -> DeviceType = 1
    ///     MIDI instrument -> DeviceType = 1 (no difference)
    ///     Audio FX        -> DeviceType = 2
    /// </summary>
    private readonly IDictionary<string, DeviceSort> deviceSortsByDeviceType = new Dictionary<string, DeviceSort>()
    {
        ["1"] = DeviceSort.MidiInstrumentOrEffect,
        ["2"] = DeviceSort.AudioEffect
    };

    public Vst3PluginFormatExtractor(ILogger<Vst3PluginFormatExtractor> logger) : base(logger, PluginFormat.VST3)
    {
    }

    protected override string PluginNameXpath => @"PluginDesc/Vst3PluginInfo/Name/@Value";

    protected override DeviceSort GetPluginSort(XPathNavigator pluginDescNode, string pluginName)
    {
        var deviceType = pluginDescNode.SelectSingleNode(@"PluginDesc/Vst3PluginInfo/DeviceType/@Value")!.Value;

        if (!deviceSortsByDeviceType.ContainsKey(deviceType))
        {
            logger.LogWarning("A plugin was found with an unknown DeviceType node value. Plugin name: {@PluginName}; DeviceType: {@DeviceType}; Expected valid device types: {@ValidDeviceTypes}.", pluginName, deviceType, deviceSortsByDeviceType.Keys);
            return DeviceSort.Unknown;
        }

        return deviceSortsByDeviceType[deviceType];
    }
}