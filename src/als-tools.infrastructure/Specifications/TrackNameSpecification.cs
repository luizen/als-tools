namespace AlsTools.Infrastructure.Specifications;

public class TrackNameSpecification : ISpecification<LiveProject>
{
    private readonly IEnumerable<string> names;
    private readonly TextMatchingOptions matchingOption;

    public TrackNameSpecification(IEnumerable<string> names, TextMatchingOptions matchingOption)
    {
        this.names = names;
        this.matchingOption = matchingOption;
    }

    public Expression<Func<LiveProject, bool>> ToExpression()
    {
        switch (matchingOption)
        {
            case TextMatchingOptions.ExactWord:
                return lp => lp.Tracks.Any(t => t.UserName.In(names));
            
            case TextMatchingOptions.StartsWith:
                return lp => lp.Tracks.Any(t => names.Any(name => t.UserName.StartsWith(name)));
            
            case TextMatchingOptions.EndsWith:
                return lp => lp.Tracks.Any(t => names.Any(name => t.UserName.EndsWith(name)));

            case TextMatchingOptions.Contains:
                return lp => lp.Tracks.Any(t => names.Any(name => t.UserName.Contains(name)));            

            default:
                throw new InvalidOperationException("Invalid TextMatchingOptions value");
        }
    }
}

