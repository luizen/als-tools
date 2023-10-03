namespace AlsTools.Core.ValueObjects.Devices.Racks;

public record AudioEffectRackDevice : BaseRackDevice
{
    public AudioEffectRackDevice() : base(DeviceSort.AudioEffectRack)
    {
    }
}