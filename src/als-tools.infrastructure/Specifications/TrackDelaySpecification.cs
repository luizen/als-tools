namespace AlsTools.Infrastructure.Specifications;

public class TrackDelaySpecification : ISpecification<LiveProject>
{
    private readonly IEnumerable<TrackDelay> _trackdelays;
    private readonly IEnumerable<double> _trackdelaysValues;

    public TrackDelaySpecification(IEnumerable<TrackDelay> trackDelays)
    {
        _trackdelays = trackDelays;
        _trackdelaysValues = trackDelays.Select(delay => delay.Value!.Value);
    }

    public Expression<Func<LiveProject, bool>> ToExpression()
    {
        return lp => lp.Tracks.Any(t => t.TrackDelay.Value!.Value.In(_trackdelaysValues));
    }
}