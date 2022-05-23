namespace AlsTools.Core.ValueObjects;

public class TrackDelay
{
    /// <summary>
    /// The delay value/amount, either in ms or samples, depending on the
    /// <see cref="IsValueSampleBased"/> property.
    /// </summary>
    public int? Value { get; set; }

    /// <summary>
    /// Whether the delay is sample based of not (in this case, milliseconds)
    /// </summary>
    public bool? IsValueSampleBased { get; set; }
}