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
                return lp => names.Any(name => lp.Name.StartsWith(name));
                // return lp => lp.Name.star
                // return lp => lp.Name.StartsWith("Techno");
                // return lp => lp.Name.StartsWith2(names);
                // return lp => names.Any(name => lp.Name.StartsWith(name));

            case TextMatchingOptions.EndsWith:
                return lp => names.Any(name => lp.Name.EndsWith(name));

            case TextMatchingOptions.Contains:
                return lp => names.Any(name => lp.Name.Contains(name));

            default:
                throw new InvalidOperationException("Invalid TextMatchingOptions value");
        }
    }
}



// public static class MyOwnRavenQueryableExtensions
// {
//     public static bool StartsWith2(this string field, IEnumerable<string> values)
//     {
//         return values.Any(value => field!.StartsWith(value));
//     }
// }