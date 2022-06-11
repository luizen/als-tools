using AlsTools.Core.ValueObjects.Devices;

namespace AlsTools.Infrastructure.Extractors.DeviceTypes.MaxForLive;

public abstract class BaseMaxForLiveDeviceSortExtractor : IMaxForLiveDeviceSortExtractor
{
    private readonly ILogger<BaseMaxForLiveDeviceSortExtractor> logger;

    private readonly DeviceSort deviceSort;

    public BaseMaxForLiveDeviceSortExtractor(ILogger<BaseMaxForLiveDeviceSortExtractor> logger, DeviceSort deviceSort)
    {
        this.logger = logger;
        this.deviceSort = deviceSort;
    }

    public IDevice ExtractFromXml(XPathNavigator pluginDescNode)
    {
        logger.LogDebug("----");
        logger.LogDebug("Extracting MaxForLive {@DeviceSort} device...", deviceSort);

        var device = new MaxForLiveDevice(deviceSort)
        {
            Id = pluginDescNode.SelectSingleNode(@"@Id")!.ValueAsInt,
            Name = GetMaxForLiveDeviceNameFromXmlNodePath(pluginDescNode.SelectSingleNode(@"SourceContext/Value/BranchSourceContext/OriginalFileRef/FileRef/Path/@Value")!.Value),
            UserName = pluginDescNode.SelectSingleNode(@"UserName/@Value")!.Value,
            Annotation = pluginDescNode.SelectSingleNode(@"Annotation/@Value")!.Value
        };

        return device;
    }

    /// <summary>
    /// Gets the MaxForLive device name from its file path
    /// </summary>
    /// <param name="nodePathValue">The path from the MaxForLive node. E.g.: "~/Documents/Production/Max4Live/Rozzer - Advanced Step Sequencer.amxd"</param>
    /// <returns>The file name from the device path</returns>
    protected string GetMaxForLiveDeviceNameFromXmlNodePath(string nodePathValue)
    {
        if (string.IsNullOrWhiteSpace(nodePathValue))
            return string.Empty;

        var fileName = Path.GetFileName(nodePathValue);
        if (!string.IsNullOrWhiteSpace(fileName))
            return fileName.Replace(".amxd", "");

        return string.Empty;
    }
}

