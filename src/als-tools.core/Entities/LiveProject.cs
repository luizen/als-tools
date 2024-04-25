using AlsTools.Core.ValueObjects;
using AlsTools.Core.ValueObjects.Tracks;

namespace AlsTools.Core.Entities;

public class LiveProject
{
    public LiveProject()
    {
        Tracks = [];
        Scenes = [];
        Locators = [];
    }

    /// <summary>
    /// Internal (persistency related) project ID.
    /// </summary>
    public int Id { get; set; } = -1;

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
    // public ICollection<ITrack> Tracks { get; set; }
    public ICollection<BaseTrack> Tracks { get; set; }

    /// <summary>
    /// The scenes this project contains
    /// </summary>
    public ICollection<Scene> Scenes { get; set; }

    /// <summary>
    /// The locators this project contains
    /// </summary>
    public ICollection<Locator> Locators { get; set; }
}