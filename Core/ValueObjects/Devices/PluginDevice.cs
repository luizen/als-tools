namespace AlsTools.Core.ValueObjects.Devices
{
    public class PluginDevice : BaseDevice, IDevice
    {
        public PluginDevice() : base(DeviceType.Plugin)
        {
        }

        public PluginType PluginType { get; set; }
    }
}
