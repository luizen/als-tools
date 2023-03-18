namespace AlsTools.Infrastructure.Specifications;

public class StockDeviceFamiliesSpecification : ISpecification<LiveProject>
{
    private readonly IEnumerable<DeviceFamily> families;

    public StockDeviceFamiliesSpecification(IEnumerable<DeviceFamily> families)
    {
        this.families = families;
    }

    public Expression<Func<LiveProject, bool>> ToExpression()
    {
        //TODO: review this
        return lp => lp.Tracks.Any(t => t.StockDevices.Any(sd => sd.Family.In(families)));
    }
}