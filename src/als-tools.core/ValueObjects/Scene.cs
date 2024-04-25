using AlsTools.Core.Entities;

namespace AlsTools.Core.ValueObjects;

public class Scene
{
    /// <summary>
    /// Gets or sets the ID of the live project associated with the scene.
    /// </summary>
    public int LiveProjectId { get; set; }

    /// <summary>
    /// Gets or sets the live project associated with the scene.
    /// </summary>
    public LiveProject LiveProject { get; set; }

    /// <summary>
    /// Scene number (Id attribute)
    /// </summary>
    public int Number { get; set; }

    /// <summary>
    /// Scene name
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Scene notes
    /// </summary>
    public string Annotation { get; set; } = string.Empty;

    /// <summary>
    /// The scene BPM/Tempo
    /// </summary>
    public int Tempo { get; set; }

    /// <summary>
    /// Whether a tempo/BPM is set
    /// </summary>
    public bool IsTempoEnabled { get; set; }

    /// <summary>
    /// The scene time signature Id. This is probably a bit mask value.
    /// </summary>
    public int TimeSignatureId { get; set; }

    /// <summary>
    /// Whether a time signature is set
    /// </summary>
    public bool IsTimeSignatureEnabled { get; set; }
}