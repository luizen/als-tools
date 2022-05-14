using System;
using System.IO;
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
    public partial class Program
    {
        private static void SetupLogging(ParserResult<object> parserResult)
        {
            Log.Debug("Setting up logging settings...");

            SetLoggingLevelFromArgs(parserResult);

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.ControlledBy(levelSwitch)
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();
        }

        private static void SetLoggingLevelFromArgs(ParserResult<object> parserResult)
        {
            Log.Debug("Setting logging level from args...");

            LoggingLevels level = LoggingLevels.Debug;
            var commonResult = parserResult.WithParsed<CommonOptions>(x => level = x.LoggingLevel);

            int levelAsNumber = ((int)level);

            levelSwitch.MinimumLevel = (LogEventLevel)levelAsNumber;
        }

        private static IHost BuildHost(string[] args)
        {
            Log.Debug("Building host...");

            return new HostBuilder()
                .ConfigureServices(ConfigureServices)
                .UseSerilog()
                .Build();
        }

        private static void ConfigureServices(IServiceCollection serviceCollection)
        {
            Log.Debug("Configuring services...");

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

            // Add some helpers
            serviceCollection.AddSingleton<UserFolderHandler>(svcProvider =>
                new UserFolderHandler(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile, Environment.SpecialFolderOption.None)));

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
