using AlsTools.Core.ValueObjects.Devices;

namespace AlsTools.Infrastructure.Extractors.DeviceTypes;

/// <summary>
/// Interface defining a extractor specific for a single IDevice
/// </summary>
public interface IDeviceExtractor
{
    IDevice ExtractFromXml(XPathNavigator deviceNode);
}