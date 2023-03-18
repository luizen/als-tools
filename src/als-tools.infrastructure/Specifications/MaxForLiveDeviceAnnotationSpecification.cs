namespace AlsTools.Infrastructure.Specifications;

public class MaxForLiveDeviceAnnotationSpecification : ISpecification<LiveProject>
{
    private readonly IEnumerable<string> annotations;

    public MaxForLiveDeviceAnnotationSpecification(IEnumerable<string> annotations)
    {
        this.annotations = annotations;
    }

    public Expression<Func<LiveProject, bool>> ToExpression()
    {
        return lp => lp.Tracks.Any(t => t.MaxForLiveDevices.Any(sd => sd.Annotation.In(annotations)));
    }
}