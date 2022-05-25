
using Raven.Client.Documents;

namespace AlsTools.Core.Interfaces;

public interface IEmbeddedDatabaseContext
{
    IDocumentStore DocumentStore { get; }
    
    void Initialize();
}
