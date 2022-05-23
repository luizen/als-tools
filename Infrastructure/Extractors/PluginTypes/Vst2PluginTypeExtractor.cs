
using System.Collections.Generic;
using System.Xml.XPath;
using AlsTools.Core.ValueObjects;
using AlsTools.Core.ValueObjects.Devices;
using Microsoft.Extensions.Logging;

namespace AlsTools.Infrastructure.Extractors.PluginTypes;

public class Vst2PluginTypeExtractor : IPluginTypeExtractor
{
    private readonly ILogger<Vst2PluginTypeExtractor> logger;

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

    public Vst2PluginTypeExtractor(ILogger<Vst2PluginTypeExtractor> logger)
    {
        this.logger = logger;
    }

    public IDevice ExtractFromXml(XPathNavigator pluginDescNode)
    {
        logger.LogDebug("Extracting VST2 plugin device...");

        var format = PluginFormat.VST2;
        var sort = GetPluginSort(pluginDescNode);
        var pluginDevice = new PluginDevice(sort, format);
        pluginDevice.Name = pluginDescNode.SelectSingleNode(@"PluginDesc/VstPluginInfo/PlugName/@Value")?.Value;
        pluginDevice.UserName = pluginDescNode.SelectSingleNode(@"UserName/@Value")?.Value;
        pluginDevice.Annotation = pluginDescNode.SelectSingleNode(@"Annotation/@Value")?.Value;
        pluginDevice.Id = pluginDescNode.SelectSingleNode(@"@Id").ValueAsInt;

        return pluginDevice;
    }

    private DeviceSort GetPluginSort(XPathNavigator pluginDescNode)
    {
        var category = pluginDescNode.SelectSingleNode(@"PluginDesc/VstPluginInfo/Category/@Value")?.Value;

        return deviceSortsByCategory[category];
    }
}