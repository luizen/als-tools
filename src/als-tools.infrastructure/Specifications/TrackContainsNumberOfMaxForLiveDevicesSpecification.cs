namespace AlsTools.Infrastructure.Specifications;

public class TrackContainsNumberOfMaxForLiveDevicesSpecification : ISpecification<LiveProject>
{
    private readonly int _numberOfMaxForLiveDevices;

    public TrackContainsNumberOfMaxForLiveDevicesSpecification(int numberOfMaxForLiveDevices)
    {
        this._numberOfMaxForLiveDevices = numberOfMaxForLiveDevices;
    }

    public Expression<Func<LiveProject, bool>> ToExpression()
    {
        return lp => lp.Tracks.Any(t => t.MaxForLiveDevices.Count == _numberOfMaxForLiveDevices);
    }
}