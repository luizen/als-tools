namespace AlsTools.Core.ValueObjects.Devices.Racks;

public abstract class BaseRackDevice : StockDevice
{
    protected BaseRackDevice(DeviceSort deviceSort) : base(deviceSort)
    {
        if (deviceSort != DeviceSort.MidiEffectRack && deviceSort != DeviceSort.AudioEffectRack && deviceSort != DeviceSort.DrumRack && deviceSort != DeviceSort.MidiInstrumentRack)
            throw new ArgumentException($"The parameter {nameof(deviceSort)} must be of any of the 'Rack' types");

        childrenDevices = new Lazy<List<IDevice>>();
    }

    private Lazy<List<IDevice>> childrenDevices;

    /// <summary>
    /// A Rack can have children devices...
    /// </summary>
    public IReadOnlyList<IDevice> ChildrenDevices => childrenDevices.Value.AsReadOnly();

    // public IList<LiveDevice> StockDevices { get; protected set; }

    // public IList<PluginDevice> Plugins { get; protected set; }

    // public IList<MaxForLiveDevice> MaxForLiveDevices { get; protected set; }

    public void AddDevice(IDevice device)
    {
        if (device == null)
            throw new ArgumentNullException(nameof(device));

        childrenDevices.Value.Add(device);
    }

    public void AddDevices(IEnumerable<IDevice> devices)
    {
        foreach (var device in devices)
            AddDevice(device);
    }

    // /// <summary>
    // /// The chains (branches) a rack contains
    // /// </summary>
    // public IList<IChain> Chains { get; protected set; }

    // public void AddChain(IChain chain)
    // {
    //     if (chain == null)
    //         throw new ArgumentNullException(nameof(chain));

    //     Chains.Add(chain);
    // }

    // public void AddChains(IEnumerable<IChain> chains)
    // {
    //     foreach (var chain in chains)
    //         AddChain(chain);
    // }
}
