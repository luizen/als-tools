namespace AlsTools.Infrastructure.Specifications;

public class PluginNameSpecification : ISpecification<LiveProject>
{
    private readonly IEnumerable<string> names;
    private readonly TextMatchingOptions matchingOption;

    public PluginNameSpecification(IEnumerable<string> names, TextMatchingOptions matchingOption)
    {
        this.names = names;
        this.matchingOption = matchingOption;
    }

    public Expression<Func<LiveProject, bool>> ToExpression()
    {
        switch (matchingOption)
        {
            case TextMatchingOptions.ExactWord:
                return lp => lp.Tracks.Any(t => t.Plugins.Any(p => p.Name.In(names)));
            
            case TextMatchingOptions.StartsWith:
                return lp => lp.Tracks.Any(t => t.Plugins.Any(p => names.Any(name => p.Name.StartsWith(name)))); //TODO: fix this, it doesn't work (exception)
            
            case TextMatchingOptions.EndsWith:
                return lp => lp.Tracks.Any(t => t.Plugins.Any(p => names.Any(name => p.Name.EndsWith(name)))); //TODO: fix this, it doesn't work (exception)

            case TextMatchingOptions.Contains:
                return lp => lp.Tracks.Any(t => t.Plugins.Any(p => names.Any(name => p.Name.Contains(name)))); //TODO: fix this, it doesn't work (exception)
                    /*
                        {System.NotSupportedException: Could not understand expression: from 'LiveProjects'.Where(lp => value(AlsTools.Infrastructure.Specifications.LiveProjectNamesSpecification).names.Any(name => lp.Name.Contains(name)))
                        ---> System.NotSupportedException: Contains is not supported, doing a substring match over a text field is a very slow operation, and is not allowed using the Linq API.
                        The recommended method is to use full text search (mark the field as Analyzed and use the Search() method to query it.
                    */

            default:
                throw new InvalidOperationException("Invalid TextMatchingOptions value");
        }

        
    }
}