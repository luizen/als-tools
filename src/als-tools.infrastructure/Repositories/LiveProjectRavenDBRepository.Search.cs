using AlsTools.Core.Entities;
using AlsTools.Core.Queries;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using Linq.PredicateBuilder;
using AlsTools.Core.Specifications;
using AlsTools.Infrastructure.Specifications;

namespace AlsTools.Infrastructure.Repositories;

public partial class LiveProjectRavenRepository
{
    public async Task<IReadOnlyList<LiveProject>> Search(QuerySpecification specification)
    {
        using (var session = store.OpenAsyncSession())
        {

            // string[] trackNames = specification.TrackQuery.UserNames.ToArray();
            // string[] pluginFormats = specification.PluginQuery.Formats.Select(f => f.ToString()).ToArray();

            bool useOrOperator = true; // Set this value based on user input
            var specs = new List<ISpecification<LiveProject>>();
            var query = session.Query<LiveProject>();

            if (specification.TrackQuery.UserNames.Any())
                specs.Add(new TrackNameSpecification(specification.TrackQuery.UserNames));

            if (specification.PluginQuery.Formats.Any())
                specs.Add(new PluginFormatSpecification(specification.PluginQuery.Formats.Select(f => f.ToString())));

            if (specs.Count > 0)
            {
                ISpecification<LiveProject> spec;

                if (specs.Count == 1)
                    spec = specs[0];
                else if (useOrOperator)
                    spec = new OrSpecification<LiveProject>(specs.ToArray());
                else
                    spec = new AndSpecification<LiveProject>(specs.ToArray());

                List<LiveProject> results = await session.Query<LiveProject>().Where(spec.ToExpression()).ToListAsync();

                return results;
            }

            return Enumerable.Empty<LiveProject>().ToList();
        }
    }

}