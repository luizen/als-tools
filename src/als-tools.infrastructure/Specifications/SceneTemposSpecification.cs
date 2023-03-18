namespace AlsTools.Infrastructure.Specifications;

public class SceneTemposSpecification : ISpecification<LiveProject>
{
    private readonly IEnumerable<int> _sceneTempos;

    public SceneTemposSpecification(IEnumerable<int> sceneTempos)
    {
        _sceneTempos = sceneTempos;
    }

    public Expression<Func<LiveProject, bool>> ToExpression()
    {
        return lp => lp.Scenes.Any(s => s.Tempo.In(_sceneTempos));
    }
}