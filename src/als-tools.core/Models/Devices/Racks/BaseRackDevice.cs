namespace AlsTools.Core.Models.Devices.Racks;

public abstract class BaseRackDevice : StockDevice
{
    protected BaseRackDevice(DeviceSort deviceSort) : base(deviceSort)
    {
        if (deviceSort != DeviceSort.MidiEffectRack && deviceSort != DeviceSort.AudioEffectRack && deviceSort != DeviceSort.DrumRack && deviceSort != DeviceSort.MidiInstrumentRack)
            throw new ArgumentException($"The parameter {nameof(deviceSort)} must be of any of the 'Rack' types");

        childrenDevices = new Lazy<List<BaseDevice>>();
    }

    private readonly Lazy<List<BaseDevice>> childrenDevices;

    /// <summary>
    /// A Rack can have children devices...
    /// </summary>
    public IReadOnlyList<BaseDevice> ChildrenDevices => childrenDevices.Value.AsReadOnly();

    public void AddDevice(BaseDevice device)
    {
        if (device == null)
            throw new ArgumentNullException(nameof(device));

        device.DefineParentRack(IsOn);

        childrenDevices.Value.Add(device);
    }

    public void AddDevices(IEnumerable<BaseDevice> devices)
    {
        foreach (var device in devices)
            AddDevice(device);
    }

    public override bool IsGroupDevice => true;
}