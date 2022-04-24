namespace AlsTools.Core.Entities.Devices
{
    public class PluginDevice : BaseDevice, IDevice
    {
        public PluginDevice() : base(DeviceType.Plugin)
        {
        }

        public PluginType PluginType { get; set; }
    }
}
