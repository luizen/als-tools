// namespace AlsTools.Core.ValueObjects.Devices.Chains;

// public interface IChain
// {
//     /// <summary>
//     /// The chain intenal Id attribute
//     /// </summary>
//     int Id { get; set; }

//     /// <summary>
//     /// The name the user specified. It can contain special values like # or ##.
//     /// Ex.: ## Chain 1
//     /// </summary>
//     string UserName { get; set; }

//     /// <summary>
//     /// The chain effective name, already expanded (if needed).
//     /// Ex.: 01 Chain 1
//     /// </summary>
//     string EffectiveName { get; set; }

//     /// <summary>
//     /// The chain info text/annotation/notes
//     /// </summary>
//     string Annotation { get; set; }

//     /// <summary>
//     /// The Ableton Live stock devices this chain contains
//     /// </summary>
//     IList<LiveDevice> StockDevices { get; }

//     /// <summary>
//     /// The third party plugins this chain contains
//     /// </summary>
//     IList<PluginDevice> Plugins { get; }

//     /// <summary>
//     /// The MaxForLive devices this chain contains
//     /// </summary>
//     IList<MaxForLiveDevice> MaxForLiveDevices { get; }

//     /// <summary>
//     /// Adds a device to either the <see cref="StockDevices" />, <see cref="Plugins" /> or <see cref="MaxForLiveDevices" />
//     /// list. Duplicated entries are allowed.
//     /// </summary>
//     /// <param name="device">The device object</param>
//     void AddDevice(IDevice device);

//     /// <summary>
//     /// Adds a list of devices to either the <see cref="StockDevices" />, <see cref="Plugins" /> or <see cref="MaxForLiveDevices" />
//     /// list. Duplicated entries are allowed.
//     /// </summary>
//     /// <param name="devices">The list of device objects</param>
//     void AddDevices(IEnumerable<IDevice> devices);
// }