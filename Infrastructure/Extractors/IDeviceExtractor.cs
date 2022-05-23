using System.Xml.XPath;
using AlsTools.Core.ValueObjects.Devices;

namespace AlsTools.Infrastructure.Extractors;

public interface IDeviceExtractor
{
    IDevice ExtractFromXml(XPathNavigator deviceNode);
}