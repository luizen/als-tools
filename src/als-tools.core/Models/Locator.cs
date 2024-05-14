namespace AlsTools.Core.Models;

public partial class Locator
{
    /// <summary>
    /// Internal (persistency related) ID.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Locator ID.
    /// </summary>
    public int LocatorId { get; set; }

    /// <summary>
    /// Locator number (Id attribute)
    /// </summary>
    public int? Number { get; set; }

    /// <summary>
    /// The time point where the location is set
    /// </summary>
    public double Time { get; set; }

    /// <summary>
    /// Locator name (displayed in arrangement window)
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Locator notes
    /// </summary>
    public string Annotation { get; set; } = string.Empty;

    /// <summary>
    /// Whether or not the locator is set as Song Start
    /// </summary>
    public bool IsSongStart { get; set; }

    /// <summary>
    /// The foreing key to the Project this Locator is part of
    /// </summary>
    public int FkProjectId { get; set; }

    /// <summary>
    /// The Project this Locator is part of
    /// </summary>
    public virtual Project FkProject { get; set; } = null!;
}