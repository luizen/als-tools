namespace AlsTools.Core.Models;

public partial class Device
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int Type { get; set; }

    public int Sort { get; set; }

    public int? Format { get; set; }

    public int FkTrackId { get; set; }

    public virtual Track FkTrack { get; set; } = null!;
}
