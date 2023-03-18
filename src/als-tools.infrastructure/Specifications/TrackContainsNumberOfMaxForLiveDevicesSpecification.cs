namespace AlsTools.Infrastructure.Specifications;

public class TrackContainsNumberOfMaxForLiveDevicesSpecification : ISpecification<LiveProject>
{
    private readonly int numberOfDevices;

    public TrackContainsNumberOfMaxForLiveDevicesSpecification(int numberOfDevices)
    {
        this.numberOfDevices = numberOfDevices;
    }

    public Expression<Func<LiveProject, bool>> ToExpression()
    {
        return lp => lp.Tracks.Any(t => t.MaxForLiveDevices.Count == numberOfDevices);
    }
}