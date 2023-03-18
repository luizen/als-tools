namespace AlsTools.Infrastructure.Specifications;

public class MaxForLiveDeviceUserNamesSpecification : ISpecification<LiveProject>
{
    private readonly IEnumerable<string> userNames;

    public MaxForLiveDeviceUserNamesSpecification(IEnumerable<string> userNames)
    {
        this.userNames = userNames;
    }

    public Expression<Func<LiveProject, bool>> ToExpression()
    {
        return lp => lp.Tracks.Any(t => t.MaxForLiveDevices.Any(sd => sd.UserName.In(userNames)));
    }
}