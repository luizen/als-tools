namespace AlsTools.Core.Interfaces;

public interface IEmbeddedDatabaseContext
{
    IDocumentStore DocumentStore { get; } // This is not right, but that's it for now...
    
    void Initialize();
}
