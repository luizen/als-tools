namespace AlsTools.Core.Models;

public partial class TrackDelay
{
    /// <summary>
    /// The delay value/amount, either in ms or samples, depending on the
    /// <see cref="IsValueSampleBased"/> property.
    /// </summary>
    public double? Value { get; set; }

    /// <summary>
    /// Whether the delay is sample based of not (in this case, milliseconds)
    /// </summary>
    public bool? IsValueSampleBased { get; set; }

    public string Description => ToString();

    override public string ToString()
    {
        return IsValueSampleBased == true ? $"{Value} samples" : $"{Value} ms";
    }
}