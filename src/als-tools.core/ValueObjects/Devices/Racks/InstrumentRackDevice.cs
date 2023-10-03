namespace AlsTools.Core.ValueObjects.Devices.Racks;

public record InstrumentRackDevice : BaseRackDevice
{
    public InstrumentRackDevice() : base(DeviceSort.MidiInstrumentRack)
    {
    }
}