namespace AlsTools.Infrastructure.Specifications;

public class StockDeviceUserNamesSpecification : ISpecification<LiveProject>
{
    private readonly IEnumerable<string> _stockDeviceNames;

    public StockDeviceUserNamesSpecification(IEnumerable<string> stockDeviceNames)
    {
        _stockDeviceNames = stockDeviceNames;
    }

    public Expression<Func<LiveProject, bool>> ToExpression()
    {
        return lp => lp.Tracks.Any(t => t.StockDevices.Any(sd => sd.UserName.In(_stockDeviceNames)));
    }
}