namespace AlsTools.Infrastructure.Specifications;

public class TrackEffectiveNameSpecification : ISpecification<LiveProject>
{
    private readonly IEnumerable<string> _trackEffectiveNames;

    public TrackEffectiveNameSpecification(IEnumerable<string> trackEffectiveNames)
    {
        _trackEffectiveNames = trackEffectiveNames;
    }

    public Expression<Func<LiveProject, bool>> ToExpression()
    {
        return lp => lp.Tracks.Any(t => t.EffectiveName.In(_trackEffectiveNames));
    }
}