namespace AlsTools.Core.Models;

public partial class Scene
{
    /// <summary>
    /// Internal (persistency related) ID.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Scene ID.
    /// </summary>
    public int SceneId { get; set; }

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

    /// <summary>
    /// Scene color
    /// </summary>
    public LiveColor Color { get; set; } = LiveColors.Unset; // this Default sometimes doesn`t work

    /// <summary>
    /// The foreing key to the Project this Scene is part of
    /// </summary>
    public int FkProjectId { get; set; }

    /// <summary>
    /// The Project this Scene is part of
    /// </summary>
    public virtual Project FkProject { get; set; } = null!;
}