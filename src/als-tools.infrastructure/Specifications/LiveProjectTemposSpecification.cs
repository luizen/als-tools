namespace AlsTools.Infrastructure.Specifications;

public class LiveProjectTemposSpecification : ISpecification<LiveProject>
{
    private readonly IEnumerable<double> tempos;

    public LiveProjectTemposSpecification(IEnumerable<double> tempos)
    {
        this.tempos = tempos;
    }

    public Expression<Func<LiveProject, bool>> ToExpression()
    {
        return lp => lp.Tempo.In(tempos);
    }
}