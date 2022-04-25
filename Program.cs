using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AlsTools.Config;
using AlsTools.Core.Interfaces;
using AlsTools.Core.Services;
using AlsTools.Infrastructure;
using AlsTools.Infrastructure.FileSystem;
using AlsTools.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Raven.Embedded;
using Serilog;
using Serilog.Events;

namespace AlsTools
{
    class Program
    {
        private static IConfigurationRoot configuration;

        private static async Task<int> Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

            Log.Debug("Building host");
            var host = BuildHost(args);

            using (var serviceScope = host.Services.CreateScope())
            {
                var services = serviceScope.ServiceProvider;

                try
                {
                    Log.Debug("Parsing arguments");
                    var arguments = ParseArguments(args);

                    Log.Debug("Starting RavenDB server");
                    var embeddedDbContext = services.GetRequiredService<IEmbeddedDatabaseContext>();
                    embeddedDbContext.Initialize();

                    Log.Debug("Starting application");
                    var app = services.GetRequiredService<AppRavenDb>();
                    await app.Run(arguments);

                    Log.Debug("Returning 0");
                    return 0;
                }
                catch (Exception ex)
                {
                    Log.Fatal(ex, "An error occured. Returning code 1");
                    return 1;
                }
                finally
                {
                    Log.CloseAndFlush();
                }
            }
        }

        public static IHost BuildHost(string[] args)
        {
            return new HostBuilder()
                .ConfigureServices(ConfigureServices)
                .UseSerilog()
                .Build();
        }

        private static void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddLogging();

            // Build configuration
            configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            // Add access to generic IConfigurationRoot
            serviceCollection.AddSingleton<IConfigurationRoot>(configuration);

            // Add DbContext
            serviceCollection.AddSingleton<ILiteDbContext, LiteDbContext>();
            serviceCollection.AddSingleton<IEmbeddedDatabaseContext, EmbeddedDatabaseContext>();

            // Add services
            serviceCollection
                .AddTransient<ILiveProjectService, LiveProjectService>()
                .AddTransient<ILiveProjectRepository, LiveProjectRepository>()
                .AddTransient<ILiveProjectAsyncService, LiveProjectAsyncService>()
                .AddTransient<ILiveProjectAsyncRepository, LiveProjectRavenRepository>()
                .AddTransient<ILiveProjectExtractor, LiveProjectExtractor>()
                .AddTransient<ILiveProjectFileSystem, LiveProjectFileSystem>();

            serviceCollection.Configure<LiteDbOptions>(configuration.GetSection("LiteDbOptions"));
            serviceCollection.Configure<RavenDbOptions>(configuration.GetSection("RavenDbOptions"));

            // Add app
            serviceCollection.AddTransient<App>();
            serviceCollection.AddTransient<AppRavenDb>();
        }

        private static ProgramArgs ParseArguments(string[] args)
        {
            var arguments = GetArguments(args);
            ValidateArguments(arguments);

            PrintArguments(arguments);

            return arguments;
        }

        private static void PrintArguments(ProgramArgs args)
        {
            Log.Debug("Parameters: {@Args}", args);
        }

        private static ProgramArgs GetArguments(string[] arguments)
        {
            var result = new ProgramArgs();
            var args = arguments.ToList();

            int indexLocate = args.FindIndex(x => x.StartsWith("--locate="));
            if (indexLocate >= 0)
            {
                var parts = args[indexLocate].Split('=');
                if (parts.Count() != 2)
                    throw new ArgumentException("Please specify a semicolon separated list of plugin names to locate!");

                result.LocatePlugins = true;
                result.PluginsToLocate = parts[1].Split(';');
            }

            if (args.IndexOf("--initdb") >= 0)
                result.InitDb = true;

            if (args.IndexOf("--count") >= 0)
                result.CountProjects = true;

            if (args.IndexOf("--list") >= 0)
                result.ListPlugins = true;

            if (args.IndexOf("--includebackups") >= 0)
                result.IncludeBackups = true;

            int indexFolder = args.FindIndex(x => x.StartsWith("--folder="));
            if (indexFolder >= 0)
            {
                var parts = args[indexFolder].Split('=');
                if (parts.Count() != 2)
                    throw new ArgumentException("Please specify a folder path!");

                result.Folder = parts[1];
            }

            int indexFile = args.FindIndex(x => x.StartsWith("--file="));
            if (indexFile >= 0)
            {
                var parts = args[indexFile].Split('=');
                if (parts.Count() != 2)
                    throw new ArgumentException("Please specify a file path!");

                result.File = parts[1];
            }

            int indexExport = args.FindIndex(x => x.StartsWith("--export="));
            if (indexExport >= 0)
            {
                var parts = args[indexFolder].Split('=');
                if (parts.Count() != 2)
                    throw new ArgumentException("Please specify a file path!");

                result.ExportFile = parts[1];
            }

            return result;
        }

        private static void ValidateArguments(ProgramArgs args)
        {
            if (args.InitDb)
            {
                // Folder or file is always mandatory for initializing the DB!
                if ((string.IsNullOrWhiteSpace(args.File) && string.IsNullOrWhiteSpace(args.Folder)) ||
                    (!string.IsNullOrWhiteSpace(args.File) && !string.IsNullOrWhiteSpace(args.Folder)))
                {
                    throw new ArgumentException("Please specify either a folder or file at least");
                }
            }

            if ((args.ListPlugins && args.LocatePlugins && args.InitDb && args.CountProjects && args.Export) ||
               (!args.ListPlugins && !args.LocatePlugins && !args.InitDb && !args.CountProjects && !args.Export))
                throw new ArgumentException("Please specify either --initdb or --count or --list or --locate or --export option");

            //TODO: implement validation of all other possibilities            
        }
    }
}
