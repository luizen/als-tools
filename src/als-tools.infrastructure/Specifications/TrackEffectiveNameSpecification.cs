namespace AlsTools.Infrastructure.Specifications;

public class TrackEffectiveNameSpecification : ISpecification<LiveProject>
{
    private readonly IEnumerable<string> effectiveNames;

    public TrackEffectiveNameSpecification(IEnumerable<string> effectiveNames)
    {
        this.effectiveNames = effectiveNames;
    }

    public Expression<Func<LiveProject, bool>> ToExpression()
    {
        return lp => lp.Tracks.Any(t => t.EffectiveName.In(effectiveNames));
    }
}