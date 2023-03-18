namespace AlsTools.Infrastructure.Specifications;

public class SceneNameSpecification : ISpecification<LiveProject>
{
    private readonly IEnumerable<string> _sceneNames;

    public SceneNameSpecification(IEnumerable<string> sceneNames)
    {
        _sceneNames = sceneNames;
    }

    public Expression<Func<LiveProject, bool>> ToExpression()
    {
        return lp => lp.Scenes.Any(s => s.Name.In(_sceneNames));
    }
}