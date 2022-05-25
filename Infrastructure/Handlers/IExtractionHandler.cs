namespace AlsTools.Infrastructure.Handlers;

public interface IExtractionHandler<T>
{
    IReadOnlyList<T> ExtractFromXml(XPathNavigator nav);
}