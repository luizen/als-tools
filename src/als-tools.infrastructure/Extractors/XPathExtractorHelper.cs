namespace AlsTools.Infrastructure.Extractors;

public class XpathExtractorHelper
{
    public bool TryGetXpathValue<T>(XPathNavigator nav, string expression, Action<T> successAction)
    {
        var node = nav.SelectSingleNode(expression);
        if (node == null)
            return false;

        var result = (T)node.ValueAs(typeof(T));

        successAction(result);

        return true;
    }

    public bool TryGetOneOfXpathValues<T>(XPathNavigator nav, string[] expressions, Action<T> successAction)
    {
        foreach (var expression in expressions)
        {
            // The first that works, we can stop and return
            if (TryGetXpathValue(nav, expression, successAction))
                return true;
        }

        return false;
    }
}