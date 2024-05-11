using AlsTools.Core.ValueObjects;
using AlsTools.Core.ValueObjects.Devices;

namespace AlsTools.Core.Models;

public abstract class MyBaseDevice
{
    public int Id { get; set; }
    public string Name { get; set; }

    // Make it get only
    public DeviceType Type { get; set; }
    public DeviceSort Sort { get; set; }
    public int FkTrackId { get; set; }
    public Track FkTrack { get; set; }
}

public class MyPluginDevice : MyBaseDevice
{
    public PluginFormat Format { get; set; }
}

public class MyStockDevice : MyBaseDevice
{
}

public class MyMaxForLiveDevice : MyBaseDevice
{
}



// public partial class Device
// {
//     public int Id { get; set; }

//     public string Name { get; set; } = null!;

//     public int Type { get; set; }

//     public int Sort { get; set; }

//     public int? Format { get; set; }

//     public int FkTrackId { get; set; }

//     public virtual Track FkTrack { get; set; } = null!;
// }
