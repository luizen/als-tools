namespace AlsTools.Core.ValueObjects.Devices.Racks;

public abstract class BaseRackDevice : StockDevice
{
    protected BaseRackDevice(DeviceSort deviceSort) : base(deviceSort)
    {
        if (deviceSort != DeviceSort.MidiEffectRack && deviceSort != DeviceSort.AudioEffectRack && deviceSort != DeviceSort.DrumRack && deviceSort != DeviceSort.MidiInstrumentRack)
            throw new ArgumentException($"The parameter {nameof(deviceSort)} must be of any of the 'Rack' types");

        childrenDevices = new Lazy<List<IDevice>>();
    }

    private readonly Lazy<List<IDevice>> childrenDevices;

    /// <summary>
    /// A Rack can have children devices...
    /// </summary>
    public IReadOnlyList<IDevice> ChildrenDevices => childrenDevices.Value.AsReadOnly();

    public void AddDevice(IDevice device)
    {
        if (device == null)
            throw new ArgumentNullException(nameof(device));

        device.HasParentRack = true;
        device.IsParentRackOn = IsOn;

        childrenDevices.Value.Add(device);
    }

    public void AddDevices(IEnumerable<IDevice> devices)
    {
        foreach (var device in devices)
            AddDevice(device);
    }

    public override bool IsGroupDevice => true;
}