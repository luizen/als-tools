using AlsTools.Core.Entities.Devices;

namespace AlsTools.Core.Factories
{
    public static class DeviceFactory
    {
        public static IDevice CreateDeviceByNodeName(string nodeName)
        {
            switch (nodeName.ToUpperInvariant())
            {
                case "PLUGINDEVICE":
                case "AUPLUGINDEVICE":
                    return new PluginDevice();

                default:
                    return new LiveDevice();
            }
        }
    }
}