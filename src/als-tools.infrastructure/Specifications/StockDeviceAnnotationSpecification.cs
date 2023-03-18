namespace AlsTools.Infrastructure.Specifications;

public class StockDeviceAnnotationSpecification : ISpecification<LiveProject>
{
    private readonly IEnumerable<string> annotations;

    public StockDeviceAnnotationSpecification(IEnumerable<string> annotations)
    {
        this.annotations = annotations;
    }

    public Expression<Func<LiveProject, bool>> ToExpression()
    {
        return lp => lp.Tracks.Any(t => t.StockDevices.Any(sd => sd.Annotation.In(annotations)));
    }
}