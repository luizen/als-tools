namespace AlsTools.Core.ValueObjects.Devices.Racks;

// This only allows MIDI effect racks as children

public class MidiEffectRackDevice : BaseRackDevice
{
    public MidiEffectRackDevice() : base(DeviceSort.MidiEffectRack)
    {
    }
}