namespace AlsTools.Infrastructure.Extractors;

public interface ICollectionExtractor<T>
{
    IReadOnlyList<T> ExtractFromXml(XPathNavigator nav);
}