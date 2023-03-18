namespace AlsTools.Infrastructure.Specifications;

public class StockDeviceNameSpecification : ISpecification<LiveProject>
{
    private readonly IEnumerable<string> names;

    public StockDeviceNameSpecification(IEnumerable<string> names)
    {
        this.names = names;
    }

    public Expression<Func<LiveProject, bool>> ToExpression()
    {
        return lp => lp.Tracks.Any(t => t.StockDevices.Any(sd => sd.Name.In(names)));
    }
}