namespace AlsTools.Infrastructure.Specifications;

public class StockDeviceUserNamesSpecification : ISpecification<LiveProject>
{
    private readonly IEnumerable<string> userNames;

    public StockDeviceUserNamesSpecification(IEnumerable<string> userNames)
    {
        this.userNames = userNames;
    }

    public Expression<Func<LiveProject, bool>> ToExpression()
    {
        return lp => lp.Tracks.Any(t => t.StockDevices.Any(sd => sd.UserName.In(userNames)));
    }
}