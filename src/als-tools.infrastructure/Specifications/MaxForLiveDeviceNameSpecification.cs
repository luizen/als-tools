namespace AlsTools.Infrastructure.Specifications;

public class MaxForLiveDeviceNameSpecification : ISpecification<LiveProject>
{
    private readonly IEnumerable<string> names;

    public MaxForLiveDeviceNameSpecification(IEnumerable<string> names)
    {
        this.names = names;
    }

    public Expression<Func<LiveProject, bool>> ToExpression()
    {
        return lp => lp.Tracks.Any(t => t.MaxForLiveDevices.Any(sd => sd.Name.In(names)));
    }
}