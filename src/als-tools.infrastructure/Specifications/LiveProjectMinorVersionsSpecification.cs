namespace AlsTools.Infrastructure.Specifications;

public class LiveProjectMinorVersionsSpecification : ISpecification<LiveProject>
{
    private readonly IEnumerable<string> minorVersions;

    public LiveProjectMinorVersionsSpecification(IEnumerable<string> minorVersions)
    {
        this.minorVersions = minorVersions;
    }

    public Expression<Func<LiveProject, bool>> ToExpression()
    {
        return lp => lp.MinorVersion.In(minorVersions);
    }
}