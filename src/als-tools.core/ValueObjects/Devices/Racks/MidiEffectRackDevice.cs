namespace AlsTools.Core.ValueObjects.Devices.Racks;

// This only allows MIDI effect racks as children

public record MidiEffectRackDevice : BaseRackDevice
{
    public MidiEffectRackDevice() : base(DeviceSort.MidiEffectRack)
    {
    }
}