// namespace AlsTools.Core.ValueObjects.Devices.Chains;

// public abstract class BaseBranch : IChain
// {
//     protected BaseBranch()
//     {
//         StockDevices = new List<LiveDevice>();
//         Plugins = new List<PluginDevice>();
//         MaxForLiveDevices = new List<MaxForLiveDevice>();
//     }

//     public int Id { get; set; }

//     public string UserName { get; set; } = string.Empty;

//     public string EffectiveName { get; set; } = string.Empty;

//     public string Annotation { get; set; } = string.Empty;

//     public IList<LiveDevice> StockDevices { get; protected set; }

//     public IList<PluginDevice> Plugins { get; protected set; }

//     public IList<MaxForLiveDevice> MaxForLiveDevices { get; protected set; }

//     public void AddDevice(IDevice device)
//     {
//         if (device == null)
//             throw new ArgumentNullException(nameof(device));

//         if (device.Family.Type == DeviceType.Plugin)
//             Plugins.Add((PluginDevice)device);
//         else if (device.Family.Type == DeviceType.Stock)
//             StockDevices.Add((LiveDevice)device);
//         else
//             MaxForLiveDevices.Add((MaxForLiveDevice)device);
//     }

//     public void AddDevices(IEnumerable<IDevice> devices)
//     {
//         foreach (var device in devices)
//             AddDevice(device);
//     }
// }