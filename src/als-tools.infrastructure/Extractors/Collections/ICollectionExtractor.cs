namespace AlsTools.Infrastructure.Extractors.Collections;

/// <summary>
/// Interface defining members for extracting a collection of type T
/// </summary>
/// <typeparam name="T">The type of object to be carried by the collection</typeparam>
public interface ICollectionExtractor<T>
{
    IReadOnlyList<T> ExtractFromXml(XPathNavigator nav);
}