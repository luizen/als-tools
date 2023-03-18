namespace AlsTools.Infrastructure.Specifications;

public class TrackAnnotationSpecification : ISpecification<LiveProject>
{
    private readonly IEnumerable<string> annotations;

    public TrackAnnotationSpecification(IEnumerable<string> annotations)
    {
        this.annotations = annotations;
    }

    public Expression<Func<LiveProject, bool>> ToExpression()
    {
        return lp => lp.Tracks.Any(t => t.Annotation.In(annotations));
    }
}