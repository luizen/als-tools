namespace AlsTools.Infrastructure.Specifications;

public class LiveProjectMajorVersionsSpecification : ISpecification<LiveProject>
{
    private readonly IEnumerable<string> majorVersions;

    public LiveProjectMajorVersionsSpecification(IEnumerable<string> majorVersions)
    {
        this.majorVersions = majorVersions;
    }

    public Expression<Func<LiveProject, bool>> ToExpression()
    {
        return lp => lp.MajorVersion.In(majorVersions);
    }
}