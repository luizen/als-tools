using AlsTools.Core.ValueObjects;

namespace AlsTools.Core.Queries;

public class TrackQuery
{
    public TrackQuery()
    {
        UserNames = Array.Empty<string>();
        EffectiveNames = Array.Empty<string>();
        Annotations = Array.Empty<string>();
        Types = Array.Empty<TrackType>();
        Delays = Array.Empty<TrackDelay>();
    }

    public IReadOnlyCollection<string> UserNames { get; set; }

    public IReadOnlyCollection<string> EffectiveNames { get; set; }

    public IReadOnlyCollection<TrackType> Types { get; set; }

    public IReadOnlyCollection<string> Annotations { get; set; }

    public bool? IsFrozen { get; set; }

    public bool? IsPartOfGroup { get; set; }

    public IReadOnlyCollection<TrackDelay> Delays { get; set; }
}