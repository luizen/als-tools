namespace AlsTools.Core.Models.Devices;

/// <summary>
/// The device family, composed of a <see cref="DeviceSort" /> and <see cref="DeviceType" />
/// </summary>
/// <remarks>
/// The combination of sort and type gives us the following combinations:
/// - Stock Audio Effect
/// - Stock MIDI Effect
/// - Stock Instrument
/// - Max4Live Audio Effect
/// - Max4Live MIDI Effect
/// - Max4Live Instrument
/// - Plugin Audio Effect
/// - Plugin MIDI effect
/// - Plugin Instrument
/// </remarks>
public class DeviceFamily
{
    public DeviceSort Sort { get; set; }

    public DeviceType Type { get; set; }

    public DeviceFamily(DeviceType type, DeviceSort sort)
    {
        Type = type;
        Sort = sort;
    }
}