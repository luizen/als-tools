namespace AlsTools.Infrastructure.Extractors.Collections;

public interface ICollectionExtractor<T>
{
    IReadOnlyList<T> ExtractFromXml(XPathNavigator nav);
}