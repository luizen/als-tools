using AlsTools.Core.Config;
using AlsTools.Core.Interfaces;
using AlsTools.Core.Services;
using AlsTools.Core.ValueObjects;
using AlsTools.Core.ValueObjects.Devices;
using AlsTools.Infrastructure;
using AlsTools.Infrastructure.Attributes;
using AlsTools.Infrastructure.Extractors;
using AlsTools.Infrastructure.Extractors.MaxForLiveSorts;
using AlsTools.Infrastructure.Extractors.PluginFormats;
using AlsTools.Infrastructure.Extractors.StockDevices;
using AlsTools.Infrastructure.Extractors.StockDevices.StockAudioEffects;
using AlsTools.Infrastructure.Extractors.StockDevices.StockInstruments;
using AlsTools.Infrastructure.Extractors.StockDevices.StockMidiEffects;
using AlsTools.Infrastructure.Extractors.StockDevices.StockRacks;
using AlsTools.Infrastructure.FileSystem;
using AlsTools.Infrastructure.Handlers;
using AlsTools.Infrastructure.Repositories;
using AlsTools.Infrastructure.XmlNodeNames;
using AlsTools.Ui.Cli.CliOptions;

namespace AlsTools.Ui.Cli;

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
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetParent(AppContext.BaseDirectory)?.FullName)
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

        // Plugin Format Extractors
        serviceCollection.AddSingleton<IDictionary<PluginFormat, IPluginFormatExtractor>>(svcProvider =>
                     new Dictionary<PluginFormat, IPluginFormatExtractor>()
                     {
                         [PluginFormat.VST2] = new Vst2PluginFormatExtractor(svcProvider.GetRequiredService<ILogger<Vst2PluginFormatExtractor>>()),
                         [PluginFormat.VST3] = new Vst3PluginFormatExtractor(svcProvider.GetRequiredService<ILogger<Vst3PluginFormatExtractor>>()),
                         [PluginFormat.AU] = new AuPluginFormatExtractor(svcProvider.GetRequiredService<ILogger<AuPluginFormatExtractor>>())
                     }
                );

        // Live Stock devices extractors
        serviceCollection.AddSingleton<ICommonStockAudioEffectExtractor, CommonStockAudioEffectExtractor>();
        serviceCollection.AddSingleton<ICommonStockMidiEffectExtractor, CommonStockMidiEffectExtractor>();
        serviceCollection.AddSingleton<ICommonStockInstrumentExtractor, CommonStockInstrumentExtractor>();

        // Live Stock Racks extractors
        serviceCollection.AddSingleton<AudioEffectRackExtractor>();
        serviceCollection.AddSingleton<MidiEffectRackExtractor>();
        serviceCollection.AddSingleton<MidiInstrumentRackExtractor>();
        serviceCollection.AddSingleton<DrumRackExtractor>();

        serviceCollection.AddSingleton<IDictionary<string, IStockDeviceExtractor>>(svcProvider => BuildStockDeviceExtractors(svcProvider));

        // Device Extractors
        serviceCollection.AddSingleton<IDictionary<DeviceType, IDeviceExtractor>>(svcProvider =>
                     new Dictionary<DeviceType, IDeviceExtractor>()
                     {
                         [DeviceType.Stock] = new StockDeviceExtractor(svcProvider.GetRequiredService<ILogger<StockDeviceExtractor>>(), svcProvider.GetRequiredService<IDictionary<string, IStockDeviceExtractor>>()),
                         [DeviceType.Plugin] = new PluginDeviceExtractor(svcProvider.GetRequiredService<ILogger<PluginDeviceExtractor>>(), svcProvider.GetRequiredService<IDictionary<PluginFormat, IPluginFormatExtractor>>()),
                         [DeviceType.MaxForLive] = new MaxForLiveDeviceExtractor(svcProvider.GetRequiredService<ILogger<MaxForLiveDeviceExtractor>>(), svcProvider.GetRequiredService<IDictionary<string, IMaxForLiveSortExtractor>>()),
                     }
                );

        // Add services
        serviceCollection
            .AddTransient<ILiveProjectAsyncService, LiveProjectAsyncService>()
                .AddTransient<ILiveProjectAsyncRepository, LiveProjectRavenRepository>()
                .AddTransient<ILiveProjectFileExtractionHandler, LiveProjectFileExtractionHandler>()
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

        // MidiEffects, MidiInstruments and AudioEffects classes
        var nestedClassTypes = typeof(LiveStockDeviceNodeNames).GetNestedTypes(BindingFlags.Static | BindingFlags.Public);

        foreach (var classType in nestedClassTypes)
        {
            // Let's check if the class has an extractor attribute already
            ExtractingStockDeviceAttribute? classExtractorAttr = classType.GetCustomAttribute<ExtractingStockDeviceAttribute>();

            // Get all public const/static fields for that class
            var fields = classType.GetFields(BindingFlags.Static | BindingFlags.Public);

            foreach (var field in fields)
            {
                // Get field value
                var key = field.GetValue(null)?.ToString()?.ToUpperInvariant();
                if (key == null)
                {
                    Log.Warning("There was an issue getting the value for the field {@Field}", field);
                    continue;
                }

                // Let's check if this field has an specific extractor attribute. Otherwise we use the one from the class...
                var fieldExtractorAttr = field.GetCustomAttribute<ExtractingStockDeviceAttribute>();

                // Now select which one to use...
                var extractorAttr = fieldExtractorAttr ?? classExtractorAttr;

                // If none of them are available, we have a problem...
                if (extractorAttr == null)
                {
                    Log.Warning("This device ({@DeviceName}) has no extractor defined", key);
                    continue;
                }

                var extractor = (IStockDeviceExtractor)svcProvider.GetRequiredService(extractorAttr.DeviceExtractorType);
                dic.Add(key, extractor);
            }
        }

        return dic;
    }
}
