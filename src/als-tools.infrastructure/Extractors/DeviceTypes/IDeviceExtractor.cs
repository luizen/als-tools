namespace AlsTools.Infrastructure.Extractors.DeviceTypes;

public interface IDeviceExtractor
{
    IDevice ExtractFromXml(XPathNavigator deviceNode);
}