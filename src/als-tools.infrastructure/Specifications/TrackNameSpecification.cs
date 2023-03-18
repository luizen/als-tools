namespace AlsTools.Infrastructure.Specifications;

public class TrackNameSpecification : ISpecification<LiveProject>
{
    private readonly IEnumerable<string> names;

    public TrackNameSpecification(IEnumerable<string> names)
    {
        this.names = names;
    }

    public Expression<Func<LiveProject, bool>> ToExpression()
    {
        return lp => lp.Tracks.Any(t => t.UserName.In(names));
    }
}