namespace AlsTools.Infrastructure.Specifications;

public class LiveProjectNamesSpecification : ISpecification<LiveProject>
{
    private readonly IEnumerable<string> names;
    private readonly TextMatchingOptions matchingOption;

    public LiveProjectNamesSpecification(IEnumerable<string> names, TextMatchingOptions matchingOption)
    {
        this.names = names;
        this.matchingOption = matchingOption;
    }

    public Expression<Func<LiveProject, bool>> ToExpression()
    {
        switch (matchingOption)
        {
            case TextMatchingOptions.ExactWord:
                return lp => lp.Name.In(names);

            case TextMatchingOptions.StartsWith:
                return lp => names.Any(name => lp.Name.StartsWith(name)); // this DOES NOT work
                // return lp => lp.Name.StartsWith("Techno");  // this works!

            case TextMatchingOptions.EndsWith:
                return lp => names.Any(name => lp.Name.EndsWith(name));

            case TextMatchingOptions.Contains:
                return lp => names.Any(name => lp.Name.Contains(name));

            default:
                throw new InvalidOperationException("Invalid TextMatchingOptions value");
        }
    }
}