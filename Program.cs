using System;
using System.IO;
using System.Threading.Tasks;
using AlsTools.CliOptions;
using AlsTools.Config;
using AlsTools.Core.Interfaces;
using AlsTools.Core.Services;
using AlsTools.Infrastructure;
using AlsTools.Infrastructure.FileSystem;
using AlsTools.Infrastructure.Repositories;
using CommandLine;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;

namespace AlsTools
{
    class Program
    {
        private static IConfigurationRoot configuration;

        private static async Task<int> Main(string[] args)
        {
            Log.Debug("Parsing arguments");
            var parserResult = Parser.Default.ParseArguments<InitDbOptions, CountOptions, ListOptions, LocateOptions>(args);
            if (parserResult.Tag == ParserResultType.NotParsed)
                return ProgramReturnCodes.CommandParseError;

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
                    Log.Debug("Starting database server");
                    var embeddedDbContext = services.GetRequiredService<IEmbeddedDatabaseContext>();
                    embeddedDbContext.Initialize();

                    Log.Debug("Starting application");
                    var app = services.GetRequiredService<App>();
                    await app.Run(parserResult);

                    Log.Debug("Returning {ReturnCode}", ProgramReturnCodes.Ok);
                    return ProgramReturnCodes.Ok;
                }
                catch (Exception ex)
                {
                    Log.Fatal(ex, "An error occurred. Returning code {ErrorCode}", ProgramReturnCodes.UnhandledError);
                    return ProgramReturnCodes.UnhandledError;
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
            serviceCollection.AddSingleton<IEmbeddedDatabaseContext, EmbeddedDatabaseContext>();

            // Add services
            serviceCollection
                .AddTransient<ILiveProjectAsyncService, LiveProjectAsyncService>()
                .AddTransient<ILiveProjectAsyncRepository, LiveProjectRavenRepository>()
                .AddTransient<ILiveProjectExtractor, LiveProjectExtractor>()
                .AddTransient<ILiveProjectFileSystem, LiveProjectFileSystem>();

            serviceCollection.Configure<DbOptions>(configuration.GetSection(nameof(DbOptions)));

            // Add app
            serviceCollection.AddTransient<App>();
        }
    }
}
