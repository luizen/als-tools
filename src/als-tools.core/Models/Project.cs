namespace AlsTools.Core.Models;

public partial class Project
{
    /// <summary>
    /// Internal (persistency related) ID.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The project name
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// The full path of this project
    /// </summary>
    public string Path { get; set; } = string.Empty;


    /// <summary>
    /// Creator ("Live version")
    /// </summary>
    public string Creator { get; set; } = string.Empty;

    /// <summary>
    /// Minor version
    /// </summary>
    public string MinorVersion { get; set; } = string.Empty;

    /// <summary>
    /// Major version
    /// </summary>
    public string MajorVersion { get; set; } = string.Empty;

    /// <summary>
    /// Schema change count
    /// </summary>
    public int? SchemaChangeCount { get; set; }

    /// <summary>
    /// The project tempo/BPM.
    /// </summary>
    public double? Tempo { get; set; }

    /// <summary>
    /// The project time signature. It is probably a bit mask.
    /// </summary>
    public int? TimeSignature { get; set; }

    /// <summary>
    /// The project global groove amount
    /// </summary>
    public double? GlobalGrooveAmount { get; set; }

    /// <summary>
    /// The tracks this project contains
    /// </summary>
    public virtual ICollection<Track> Tracks { get; set; } = [];

    /// <summary>
    /// The scenes this project contains
    /// </summary>
    public virtual ICollection<Scene> Scenes { get; set; }

    /// <summary>
    /// The locators this project contains
    /// </summary>
    public virtual ICollection<Locator> Locators { get; set; }
}
