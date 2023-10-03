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

    public IDevice ExtractFromXml(XPathNavigator deviceNode)
    {
        logger.LogDebug("----");
        logger.LogDebug("Extracting MaxForLive {@DeviceSort} device...", deviceSort);

        var device = new MaxForLiveDevice(deviceSort)
        {
            Id = deviceNode.SelectSingleNode(@"@Id")!.ValueAsInt,
            Name = GetMaxForLiveDeviceNameFromXmlFileRefNode(deviceNode),
            UserName = deviceNode.SelectSingleNode(@"UserName/@Value")!.Value,
            Annotation = deviceNode.SelectSingleNode(@"Annotation/@Value")!.Value,
            IsOn = deviceNode.SelectSingleNode(@"On/Manual/@Value")!.ValueAsBoolean
        };

        return device;
    }

    /// <summary>
    /// Gets the MaxForLive device name from its file path
    /// </summary>
    /// <param name="deviceNode">The XPathNavigator instance pointing to the device node"</param>
    /// <returns>The file name from the device path</returns>
    protected string GetMaxForLiveDeviceNameFromXmlFileRefNode(XPathNavigator deviceNode)
    {
        string? nodePathValue = deviceNode.SelectSingleNode(@"SourceContext/Value/BranchSourceContext/OriginalFileRef/FileRef/Path/@Value")?.Value ??
                                deviceNode.SelectSingleNode(@"SourceContext/Value/BranchSourceContext/OriginalFileRef/FileRef/Name/@Value")?.Value;

        if (string.IsNullOrWhiteSpace(nodePathValue))
            return string.Empty;

        var fileName = Path.GetFileName(nodePathValue);

        return string.IsNullOrWhiteSpace(fileName) ? string.Empty : fileName.Replace(".amxd", "");
    }
}


