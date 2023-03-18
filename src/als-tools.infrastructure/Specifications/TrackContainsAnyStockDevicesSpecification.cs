namespace AlsTools.Infrastructure.Specifications;

public class TrackContainsAnyStockDevicesSpecification : ISpecification<LiveProject>
{
    public TrackContainsAnyStockDevicesSpecification()
    {
    }

    public Expression<Func<LiveProject, bool>> ToExpression()
    {
        return lp => lp.Tracks.Any(t => t.StockDevices.Any());
    }
}