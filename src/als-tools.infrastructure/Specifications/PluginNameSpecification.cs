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
                return lp => lp.Tracks.Any(t => t.Plugins.Any(p => names.Any(name => p.Name.EndsWith(name))));

            case TextMatchingOptions.Contains:
                return lp => lp.Tracks.Any(t => names.Any(name => t.UserName.Contains(name)));

            default:
                throw new InvalidOperationException("Invalid TextMatchingOptions value");
        }

        
    }
}