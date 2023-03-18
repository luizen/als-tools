namespace AlsTools.Infrastructure.Specifications;

public class SceneNameSpecification : ISpecification<LiveProject>
{
    private readonly IEnumerable<string> names;

    public SceneNameSpecification(IEnumerable<string> names)
    {
        this.names = names;
    }

    public Expression<Func<LiveProject, bool>> ToExpression()
    {
        return lp => lp.Scenes.Any(s => s.Name.In(names));
    }
}