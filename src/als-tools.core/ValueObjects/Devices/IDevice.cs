namespace AlsTools.Core.ValueObjects.Devices;

public interface IDevice
{
    int Id { get; set; }

    string Name { get; set; }

    DeviceFamily Family { get; }
}
