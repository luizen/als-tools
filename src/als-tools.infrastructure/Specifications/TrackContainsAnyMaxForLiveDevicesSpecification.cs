namespace AlsTools.Infrastructure.Specifications;

public class TrackContainsAnyMaxForLiveDevicesSpecification : ISpecification<LiveProject>
{
    public TrackContainsAnyMaxForLiveDevicesSpecification()
    {
    }

    public Expression<Func<LiveProject, bool>> ToExpression()
    {
        return lp => lp.Tracks.Any(t => t.MaxForLiveDevices.Any());
    }
}