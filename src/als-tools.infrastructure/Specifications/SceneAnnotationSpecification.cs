namespace AlsTools.Infrastructure.Specifications;

public class SceneAnnotationSpecification : ISpecification<LiveProject>
{
    private readonly IEnumerable<string> _sceneAnnotations;

    public SceneAnnotationSpecification(IEnumerable<string> sceneAnnotations)
    {
        _sceneAnnotations = sceneAnnotations;
    }

    public Expression<Func<LiveProject, bool>> ToExpression()
    {
        return lp => lp.Scenes.Any(s => s.Annotation.In(_sceneAnnotations));
    }
}