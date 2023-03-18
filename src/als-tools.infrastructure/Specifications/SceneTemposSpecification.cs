namespace AlsTools.Infrastructure.Specifications;

public class SceneTemposSpecification : ISpecification<LiveProject>
{
    private readonly IEnumerable<int> tempos;

    public SceneTemposSpecification(IEnumerable<int> tempos)
    {
        this.tempos = tempos;
    }

    public Expression<Func<LiveProject, bool>> ToExpression()
    {
        return lp => lp.Scenes.Any(s => s.Tempo.In(tempos));
    }
}