namespace AlsTools.Infrastructure.Specifications;

public class SceneAnnotationSpecification : ISpecification<LiveProject>
{
    private readonly IEnumerable<string> annotations;

    public SceneAnnotationSpecification(IEnumerable<string> annotations)
    {
        this.annotations = annotations;
    }

    public Expression<Func<LiveProject, bool>> ToExpression()
    {
        return lp => lp.Scenes.Any(s => s.Annotation.In(annotations));
    }
}