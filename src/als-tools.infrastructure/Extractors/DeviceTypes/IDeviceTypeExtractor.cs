using AlsTools.Core.ValueObjects.Devices;

namespace AlsTools.Infrastructure.Extractors.DeviceTypes;

public interface IDeviceTypeExtractor
{
    IDevice ExtractFromXml(XPathNavigator deviceNode);
}