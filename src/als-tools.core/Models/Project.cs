namespace AlsTools.Core.Models;

public partial class Project
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Path { get; set; }

    public int? Tempo { get; set; }

    public virtual ICollection<Track> Tracks { get; set; } = new List<Track>();
}
