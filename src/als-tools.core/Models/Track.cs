namespace AlsTools.Core.Models;

public partial class Track
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int Type { get; set; }

    public int FkProjectId { get; set; }

    public virtual ICollection<Device> Devices { get; set; } = new List<Device>();

    public virtual Project FkProject { get; set; } = null!;
}
