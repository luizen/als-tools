using AlsTools.Core.Entities;

namespace AlsTools.Core.ValueObjects;

public class Locator
{
    /// <summary>
    /// Gets or sets the ID of the live project associated with the locator.
    /// </summary>
    public int LiveProjectId { get; set; }

    /// <summary>
    /// Gets or sets the live project associated with the locator.
    /// </summary>
    public LiveProject LiveProject { get; set; }

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
}