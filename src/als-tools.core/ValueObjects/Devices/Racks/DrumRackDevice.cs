namespace AlsTools.Core.ValueObjects.Devices.Racks;

public record DrumRackDevice : BaseRackDevice
{
    public DrumRackDevice() : base(DeviceSort.DrumRack)
    {
    }
}