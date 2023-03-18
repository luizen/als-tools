namespace AlsTools.Infrastructure.Specifications;

public class StockDeviceNameSpecification : ISpecification<LiveProject>
{
    private readonly IEnumerable<string> _stockDeviceNames;

    public StockDeviceNameSpecification(IEnumerable<string> stockDeviceNames)
    {
        _stockDeviceNames = stockDeviceNames;
    }

    public Expression<Func<LiveProject, bool>> ToExpression()
    {
        return lp => lp.Tracks.Any(t => t.StockDevices.Any(sd => sd.Name.In(_stockDeviceNames)));
    }
}