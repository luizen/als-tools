namespace AlsTools.Infrastructure.Specifications;

public class TrackDelaySpecification : ISpecification<LiveProject>
{
    private readonly IEnumerable<TrackDelay> delays;
    private readonly IEnumerable<double> delaysValues;

    public TrackDelaySpecification(IEnumerable<TrackDelay> delays)
    {
        this.delays = delays;
        delaysValues = delays.Select(delay => delay.Value!.Value);
    }

    public Expression<Func<LiveProject, bool>> ToExpression()
    {
        return lp => lp.Tracks.Any(t => t.TrackDelay.Value!.Value.In(delaysValues));
    }
}