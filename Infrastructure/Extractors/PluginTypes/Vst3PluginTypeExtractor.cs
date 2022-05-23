
using System.Collections.Generic;
using System.Xml.XPath;
using AlsTools.Core.ValueObjects;
using AlsTools.Core.ValueObjects.Devices;
using Microsoft.Extensions.Logging;

namespace AlsTools.Infrastructure.Extractors.PluginTypes;

public class Vst3PluginTypeExtractor : IPluginTypeExtractor
{
    private readonly ILogger<Vst3PluginTypeExtractor> logger;

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

    public Vst3PluginTypeExtractor(ILogger<Vst3PluginTypeExtractor> logger)
    {
        this.logger = logger;
    }

    public IDevice ExtractFromXml(XPathNavigator pluginDescNode)
    {
        logger.LogDebug("Extracting VST3 plugin device...");

        var format = PluginFormat.VST3;
        var sort = GetPluginSort(pluginDescNode);
        var pluginDevice = new PluginDevice(sort, format);        
        pluginDevice.Name = pluginDescNode.SelectSingleNode(@"PluginDesc/Vst3PluginInfo/Name/@Value")?.Value;        
        pluginDevice.UserName = pluginDescNode.SelectSingleNode(@"UserName/@Value")?.Value;
        pluginDevice.Annotation = pluginDescNode.SelectSingleNode(@"Annotation/@Value")?.Value;
        pluginDevice.Id = pluginDescNode.SelectSingleNode(@"@Id").ValueAsInt;

        return pluginDevice;
    }

    private DeviceSort GetPluginSort(XPathNavigator pluginDescNode)
    {
        var deviceType = pluginDescNode.SelectSingleNode(@"Vst3PluginInfo/DeviceType/@Value")?.Value;
        return deviceSortsByDeviceType[deviceType];
    }
}