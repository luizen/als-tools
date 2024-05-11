namespace AlsTools.Core.Models;

public partial class Track
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int Type { get; set; }

    public int FkProjectId { get; set; }

    // public virtual ICollection<Device> Devices { get; set; } = new List<Device>();
    public virtual ICollection<MyPluginDevice> PluginDevices { get; set; } = new List<MyPluginDevice>();
    public virtual ICollection<MyStockDevice> StockDevices { get; set; } = new List<MyStockDevice>();
    public virtual ICollection<MyMaxForLiveDevice> MaxForLiveDevices { get; set; } = new List<MyMaxForLiveDevice>();

    public virtual Project FkProject { get; set; } = null!;
}
