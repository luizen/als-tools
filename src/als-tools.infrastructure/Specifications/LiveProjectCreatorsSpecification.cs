namespace AlsTools.Infrastructure.Specifications;

public class LiveProjectCreatorsSpecification : ISpecification<LiveProject>
{
    private readonly IEnumerable<string> creators;

    public LiveProjectCreatorsSpecification(IEnumerable<string> creators)
    {
        this.creators = creators;
    }

    public Expression<Func<LiveProject, bool>> ToExpression()
    {
        return lp => lp.Creator.In(creators);
    }
}