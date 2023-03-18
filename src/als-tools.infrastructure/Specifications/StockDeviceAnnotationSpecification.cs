namespace AlsTools.Infrastructure.Specifications;

public class StockDeviceAnnotationSpecification : ISpecification<LiveProject>
{
    private readonly IEnumerable<string> _stockDeviceAnnotations;

    public StockDeviceAnnotationSpecification(IEnumerable<string> stockDeviceAnnotations)
    {
        _stockDeviceAnnotations = stockDeviceAnnotations;
    }

    public Expression<Func<LiveProject, bool>> ToExpression()
    {
        return lp => lp.Tracks.Any(t => t.StockDevices.Any(sd => sd.Annotation.In(_stockDeviceAnnotations)));
    }
}