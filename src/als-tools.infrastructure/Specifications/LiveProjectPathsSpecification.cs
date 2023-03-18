namespace AlsTools.Infrastructure.Specifications;

public class LiveProjectPathsSpecification : ISpecification<LiveProject>
{
    private readonly IEnumerable<string> paths;

    public LiveProjectPathsSpecification(IEnumerable<string> paths)
    {
        this.paths = paths;
    }

    public Expression<Func<LiveProject, bool>> ToExpression()
    {
        return lp => lp.Path.In(paths);
    }
}