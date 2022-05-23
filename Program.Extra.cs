using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using AlsTools.CliOptions;
using AlsTools.Config;
using AlsTools.Core.Interfaces;
using AlsTools.Core.Services;
using AlsTools.Core.ValueObjects.Devices;
using AlsTools.Infrastructure;
using AlsTools.Infrastructure.Extractors;
using AlsTools.Infrastructure.Extractors.MaxForLiveSorts;
using AlsTools.Infrastructure.Extractors.PluginTypes;
using AlsTools.Infrastructure.Extractors.StockDevices;
using AlsTools.Infrastructure.FileSystem;
using AlsTools.Infrastructure.FileSystem.Extractors.StockDevices.StockAudioEffects;
using AlsTools.Infrastructure.Handlers;
using AlsTools.Infrastructure.Repositories;
using AlsTools.Infrastructure.XmlNodeNames;
using CommandLine;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;

namespace AlsTools;

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

        // MaxForLive Extractors
        serviceCollection.AddSingleton<IDictionary<string, IMaxForLiveSortExtractor>>(svcProvider =>
                     new Dictionary<string, IMaxForLiveSortExtractor>()
                     {
                         [DeviceTypeNodeName.MaxForLiveAudioEffect] = new MaxForLiveAudioEffectExtractor(svcProvider.GetRequiredService<ILogger<MaxForLiveAudioEffectExtractor>>()),
                         [DeviceTypeNodeName.MaxForLiveInstrument] = new MaxForLiveMidiInstrumentExtractor(svcProvider.GetRequiredService<ILogger<MaxForLiveMidiInstrumentExtractor>>()),
                         [DeviceTypeNodeName.MaxForLiveMidiEffect] = new MaxForLiveMidiEffectExtractor(svcProvider.GetRequiredService<ILogger<MaxForLiveMidiEffectExtractor>>())
                     }
                );

        // Plugin Extractors
        serviceCollection.AddSingleton<IDictionary<string, IPluginTypeExtractor>>(svcProvider =>
                     new Dictionary<string, IPluginTypeExtractor>()
                     {
                         [PluginTypeNodeName.VST2] = new Vst2PluginTypeExtractor(svcProvider.GetRequiredService<ILogger<Vst2PluginTypeExtractor>>()),
                         [PluginTypeNodeName.VST3] = new Vst3PluginTypeExtractor(svcProvider.GetRequiredService<ILogger<Vst3PluginTypeExtractor>>()),
                         [PluginTypeNodeName.AU] = new AuPluginTypeExtractor(svcProvider.GetRequiredService<ILogger<AuPluginTypeExtractor>>())
                     }
                );

        // Live Stock devices extractors
        serviceCollection.AddSingleton<IBaseStockAudioEffect, BaseStockAudioEffect>();
        serviceCollection.AddSingleton<IBaseStockMidiEffect, BaseStockMidiEffect>();
        serviceCollection.AddSingleton<IBaseStockInstrument, BaseStockInstrument>();
        serviceCollection.AddSingleton<IDictionary<string, IStockDeviceExtractor>>(svcProvider => BuildStockDeviceExtractors(svcProvider));

        // Device Extractors
        serviceCollection.AddSingleton<IDictionary<DeviceType, IDeviceExtractor>>(svcProvider =>
                     new Dictionary<DeviceType, IDeviceExtractor>()
                     {
                         [DeviceType.Stock] = new StockDeviceExtractor(svcProvider.GetRequiredService<ILogger<StockDeviceExtractor>>(), svcProvider.GetRequiredService<IDictionary<string, IStockDeviceExtractor>>()),
                         [DeviceType.Plugin] = new PluginDeviceExtractor(svcProvider.GetRequiredService<ILogger<PluginDeviceExtractor>>(), svcProvider.GetRequiredService<IDictionary<string, IPluginTypeExtractor>>()),
                         [DeviceType.MaxForLive] = new MaxForLiveDeviceExtractor(svcProvider.GetRequiredService<ILogger<MaxForLiveDeviceExtractor>>(), svcProvider.GetRequiredService<IDictionary<string, IMaxForLiveSortExtractor>>()),
                     }
                );

        // Add services
        serviceCollection
            .AddTransient<ILiveProjectAsyncService, LiveProjectAsyncService>()
                .AddTransient<ILiveProjectAsyncRepository, LiveProjectRavenRepository>()
                .AddTransient<ILiveProjectExtractor, LiveProjectExtractor>()
                .AddTransient<ILiveProjectFileSystem, LiveProjectFileSystem>()
                .AddTransient<ILiveProjectExtractionHandler, LiveProjectExtractionHandler>()
                .AddTransient<IDeviceExtractionHandler, DeviceExtractionHandler>()
                .AddTransient<ILocatorExtractionHandler, LocatorExtractionHandler>()
                .AddTransient<ISceneExtractionHandler, SceneExtractionHandler>()
                .AddTransient<ITrackExtractionHandler, TrackExtractionHandler>();

        serviceCollection.Configure<DbOptions>(configuration.GetSection(nameof(DbOptions)));

        // Add app
        serviceCollection.AddTransient<App>();
    }
    private static IDictionary<string, IStockDeviceExtractor> BuildStockDeviceExtractors(IServiceProvider svcProvider)
    {
        var dic = new Dictionary<string, IStockDeviceExtractor>();

        AddStockDeviceExtractorsFromNodeNamesType(dic, typeof(LiveStockDeviceNodeName.AudioEffects), svcProvider.GetRequiredService<IBaseStockAudioEffect>());
        AddStockDeviceExtractorsFromNodeNamesType(dic, typeof(LiveStockDeviceNodeName.MidiEffects), svcProvider.GetRequiredService<IBaseStockMidiEffect>());
        AddStockDeviceExtractorsFromNodeNamesType(dic, typeof(LiveStockDeviceNodeName.MidiInstruments), svcProvider.GetRequiredService<IBaseStockInstrument>());

        return dic;
    }
    private static void AddStockDeviceExtractorsFromNodeNamesType(IDictionary<string, IStockDeviceExtractor> dic, Type nodeNamesType, IStockDeviceExtractor extractor)
    {
        var fields = nodeNamesType.GetFields(BindingFlags.Static | BindingFlags.Public);

        foreach (var field in fields)
        {
            var key = field.GetValue(null).ToString().ToUpperInvariant();
            dic.Add(key, extractor);
        }
    }
}
