namespace AlsTools.Infrastructure.Specifications;

public class MaxForLiveDeviceFamiliesSpecification : ISpecification<LiveProject>
{
    private readonly IEnumerable<DeviceFamily> families;

    public MaxForLiveDeviceFamiliesSpecification(IEnumerable<DeviceFamily> families)
    {
        this.families = families;
    }

    public Expression<Func<LiveProject, bool>> ToExpression()
    {
        //TODO: review this
        return lp => lp.Tracks.Any(t => t.MaxForLiveDevices.Any(sd => sd.Family.In(families)));
    }
}