// using AlsTools.Core.Config;
// using AlsTools.Core.Interfaces;
// using Raven.Client.Documents;
// using Raven.Client.Documents.Indexes;
// using Raven.Embedded;

// namespace AlsTools.Infrastructure;

// public class EmbeddedDatabaseContext : IEmbeddedDatabaseContext
// {
//     private readonly ILogger<EmbeddedDatabaseContext> logger;
//     private readonly IOptions<DbOptions> options;
//     private bool isInitialized = false;
//     private IDocumentStore? documentStore;

//     public IDocumentStore DocumentStore
//     {
//         get
//         {
//             if (!isInitialized)
//                 throw new InvalidOperationException("It is not possible to get the DocumentStore because the Embedded DB Server has not been started yet.");

//             documentStore ??= EmbeddedServer.Instance.GetDocumentStore(options.Value.DocumentStoreName);

//             return documentStore;
//         }
//     }

//     public EmbeddedDatabaseContext(ILogger<EmbeddedDatabaseContext> logger, IOptions<DbOptions> options)
//     {
//         this.logger = logger;
//         this.options = options;
//     }

//     public void Initialize()
//     {
//         logger.LogDebug("Starting database server...");

//         EmbeddedServer.Instance.StartServer(new ServerOptions
//         {
//             ServerUrl = options.Value.ServerUrl,
//             DataDirectory = Path.Combine(AppContext.BaseDirectory, options.Value.DataLocation)
//         });

//         isInitialized = true;

//         DeployAllIndexes();
//     }

//     private void DeployAllIndexes()
//     {
//         logger.LogDebug("Deploying database indexes...");

//         // If an index already exists, it is ignored
//         IndexCreation.CreateIndexes(typeof(EmbeddedDatabaseContext).Assembly, DocumentStore);
//     }
// }
