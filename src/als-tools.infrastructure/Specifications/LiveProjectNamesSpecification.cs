namespace AlsTools.Infrastructure.Specifications;

public class LiveProjectNamesSpecification : ISpecification<LiveProject>
{
    private readonly IEnumerable<string> names;

    public LiveProjectNamesSpecification(IEnumerable<string> names)
    {
        this.names = names;
    }

    public Expression<Func<LiveProject, bool>> ToExpression()
    {
        return lp => lp.Name.In(names);
    }
}