using AlsTools.Core.ValueObjects;
using AlsTools.Core.ValueObjects.Devices;

namespace AlsTools.Infrastructure.Extractors.PluginFormats;

public class Vst3PluginFormatExtractor : BasePluginFormatExtractor, IPluginFormatExtractor
{
    private readonly ILogger<Vst3PluginFormatExtractor> logger;

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
        this.logger = logger;
    }

    protected override string PluginNameXpath => @"PluginDesc/Vst3PluginInfo/Name/@Value";

    protected override DeviceSort GetPluginSort(XPathNavigator pluginDescNode, string pluginName)
    {
        var deviceType = pluginDescNode.SelectSingleNode(@"PluginDesc/Vst3PluginInfo/DeviceType/@Value")?.Value;

        if (!deviceSortsByDeviceType.ContainsKey(deviceType))
        {
            logger.LogWarning("A plugin was found with an unknown DeviceType node value. Plugin name: {@PluginName}; DeviceType: {@DeviceType}; Expected valid device types: {@ValidDeviceTypes}.", pluginName, deviceType, deviceSortsByDeviceType.Keys);
            return DeviceSort.Unknown;
        }

        return deviceSortsByDeviceType[deviceType];
    }
}