
using AlsTools.Core.Interfaces;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Session;

public static class RavenDbExtensions
{
    public static IRavenQueryable<TResult> Limit<TResult>(this IRavenQueryable<TResult> query, int? limit)
    {
        return limit.HasValue ? query.Take(limit.Value) : query;
    }

    public static IRavenQueryable<TResult> GetIgnoreDisabledQuery<TResult, TIndex>(this IAsyncDocumentSession session, bool ignoreDisabled)
        where TResult : IEnabledResultSet where TIndex : AbstractGenericIndexCreationTask<TResult>, new()
    {
        return ignoreDisabled
            ? session.Query<TResult, TIndex>().Where(result => result.IsEnabled)
            : session.Query<TResult, TIndex>();
    }
}