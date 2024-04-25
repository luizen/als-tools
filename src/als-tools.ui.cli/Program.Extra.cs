using AlsTools.Infrastructure.Extractors;
using AlsTools.Ui.Cli.OptionCommandHandlers.Handlers;
using Microsoft.EntityFrameworkCore;

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

    private static void ConfigureServices(IServiceCollection services)
    {
        Log.Debug("Configuring services...");

        services.AddLogging();

        // Build configuration
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetParent(AppContext.BaseDirectory)!.FullName)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        // Add access to generic IConfigurationRoot
        services.AddSingleton<IConfigurationRoot>(configuration);

        // Add DbContext
        services.AddDbContext<AlsToolsDbContext>(options =>
            options.UseSqlite(configuration.GetConnectionString("DefaultConnection")));

        // Add some helpers
        services.AddSingleton<UserFolderHandler>(svcProvider =>
            new UserFolderHandler(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile, Environment.SpecialFolderOption.None)));

        services.AddSingleton<XpathExtractorHelper>();

        // MaxForLive Sort Extractors
        services.AddSingleton<IDictionary<string, IMaxForLiveDeviceSortExtractor>>(svcProvider =>
                     new Dictionary<string, IMaxForLiveDeviceSortExtractor>()
                     {
                         [DeviceTypeNodeName.MaxForLiveAudioEffect] = new MaxForLiveAudioEffectDeviceSortExtractor(svcProvider.GetRequiredService<ILogger<MaxForLiveAudioEffectDeviceSortExtractor>>()),
                         [DeviceTypeNodeName.MaxForLiveInstrument] = new MaxForLiveMidiInstrumentDeviceSortExtractor(svcProvider.GetRequiredService<ILogger<MaxForLiveMidiInstrumentDeviceSortExtractor>>()),
                         [DeviceTypeNodeName.MaxForLiveMidiEffect] = new MaxForLiveMidiEffectDeviceSortExtractor(svcProvider.GetRequiredService<ILogger<MaxForLiveMidiEffectDeviceSortExtractor>>())
                     }
                );

        // Plugin Format Extractors
        services.AddSingleton<IDictionary<PluginFormat, IPluginFormatExtractor>>(svcProvider =>
                     new Dictionary<PluginFormat, IPluginFormatExtractor>()
                     {
                         [PluginFormat.VST2] = new Vst2PluginFormatExtractor(svcProvider.GetRequiredService<ILogger<Vst2PluginFormatExtractor>>()),
                         [PluginFormat.VST3] = new Vst3PluginFormatExtractor(svcProvider.GetRequiredService<ILogger<Vst3PluginFormatExtractor>>()),
                         [PluginFormat.AUv2] = new AuV2PluginFormatExtractor(svcProvider.GetRequiredService<ILogger<AuV2PluginFormatExtractor>>())//,
                                                                                                                                                  //  [PluginFormat.AUv3] = new AuV3PluginFormatExtractor(svcProvider.GetRequiredService<ILogger<AuV3PluginFormatExtractor>>())
                     }
                );

        // Device Types by Node desc
        services.AddSingleton<IDictionary<string, DeviceType>>(svcProvider =>
                     new Dictionary<string, DeviceType>()
                     {
                         [DeviceTypeNodeName.Plugin] = DeviceType.Plugin,
                         [DeviceTypeNodeName.AuPlugin] = DeviceType.Plugin,
                         [DeviceTypeNodeName.MaxForLiveAudioEffect] = DeviceType.MaxForLive,
                         [DeviceTypeNodeName.MaxForLiveInstrument] = DeviceType.MaxForLive,
                         [DeviceTypeNodeName.MaxForLiveMidiEffect] = DeviceType.MaxForLive
                     }
                );

        // Common Live stock devices extractors
        services.AddSingleton<ICommonStockAudioEffectDeviceExtractor, CommonStockAudioEffectDeviceExtractor>();
        services.AddSingleton<ICommonStockMidiEffectDeviceExtractor, CommonStockMidiEffectDeviceExtractor>();
        services.AddSingleton<ICommonStockInstrumentDeviceExtractor, CommonStockInstrumentDeviceExtractor>();

        // Live Stock Racks extractors
        services.AddSingleton<AudioEffectRackDeviceExtractor>();
        services.AddSingleton<MidiEffectRackDeviceExtractor>();
        services.AddSingleton<MidiInstrumentRackDeviceExtractor>();
        services.AddSingleton<DrumRackDeviceExtractor>();

        // All Live Stock device extractors by their XML node names
        services.AddSingleton<IDictionary<string, IStockDeviceExtractor>>(svcProvider => BuildStockDeviceExtractors(svcProvider));

        // Device type extractors
        services.AddSingleton<Lazy<IDictionary<DeviceType, IDeviceTypeExtractor>>>(svcProvider =>
                     new Lazy<IDictionary<DeviceType, IDeviceTypeExtractor>>(() =>
                        new Dictionary<DeviceType, IDeviceTypeExtractor>()
                        {
                            [DeviceType.Stock] = new StockDeviceDeviceTypeExtractor(svcProvider.GetRequiredService<ILogger<StockDeviceDeviceTypeExtractor>>(), svcProvider.GetRequiredService<IDictionary<string, IStockDeviceExtractor>>()),
                            [DeviceType.Plugin] = new PluginDeviceTypeExtractor(svcProvider.GetRequiredService<ILogger<PluginDeviceTypeExtractor>>(), svcProvider.GetRequiredService<IDictionary<PluginFormat, IPluginFormatExtractor>>()),
                            [DeviceType.MaxForLive] = new MaxForLiveDeviceTypeExtractor(svcProvider.GetRequiredService<ILogger<MaxForLiveDeviceTypeExtractor>>(), svcProvider.GetRequiredService<IDictionary<string, IMaxForLiveDeviceSortExtractor>>()),
                        }
                     )


                );

        // Add services
        services
            .AddSingleton<ILiveProjectAsyncService, LiveProjectAsyncService>()
            .AddSingleton<ILiveProjectAsyncRepository, LiveProjectEfCoreRepository>()
            .AddSingleton<ILiveProjectFileExtractionHandler, LiveProjectFileExtractionHandler>()
            .AddSingleton<ILiveProjectFileSystem, LiveProjectFileSystem>()
            .AddSingleton<ILiveProjectsCollectionExtractor, LiveProjectsCollectionExtractor>()
            .AddSingleton<IDevicesCollectionExtractor, DevicesCollectionExtractor>()
            .AddSingleton<ILocatorsCollectionExtractor, LocatorsCollectionExtractor>()
            .AddSingleton<IScenesCollectionExtractor, ScenesCollectionExtractor>()
            .AddSingleton<ITracksCollectionExtractor, TracksCollectionExtractor>();

        // Add CLI command handlers
        services
            .AddSingleton<IOptionCommandHandler<InitDbOptions>, InitDbCommandHandler>()
            .AddSingleton<IOptionCommandHandler<ListOptions>, ListCommandHandler>()
            .AddSingleton<IOptionCommandHandler<CountOptions>, CountCommandHandler>()
            .AddSingleton<IOptionCommandHandler<LocateOptions>, LocateCommandHandler>()
            .AddSingleton<IOptionCommandHandler<PluginUsageOptions>, PluginUsageCommandHandler>()
            .AddSingleton<IOptionCommandHandler<PrintStatisticsOptions>, PrintStatisticsCommandHandler>()
            .AddSingleton<ProjectsAndPluginsPrinter>();

        // PlugInfo options
        services.Configure<PlugInfoOptions>(configuration.GetSection(nameof(PlugInfoOptions)));

        // PlugScanning options
        services.Configure<PlugScanningOptions>(configuration.GetSection(nameof(PlugScanningOptions)));

        // ParameterValues options
        services.Configure<ParameterValuesOptions>(configuration.GetSection(nameof(ParameterValuesOptions)));

        // Add app
        services.AddTransient<App>();
    }

    private static IDictionary<string, IStockDeviceExtractor> BuildStockDeviceExtractors(IServiceProvider svcProvider)
    {
        Log.Debug("Starting BuildStockDeviceExtractors()...");

        var dic = new Dictionary<string, IStockDeviceExtractor>();

        // MidiEffects, MidiInstruments and AudioEffects classes
        var nestedClassTypes = typeof(LiveStockDeviceNodeNames).GetNestedTypes(BindingFlags.Static | BindingFlags.Public);

        Log.Debug("Starting foreach in nestedClassTypes...");
        foreach (var classType in nestedClassTypes)
        {
            Log.Debug(@"ClassType: {@ClassType}", classType.FullName);

            // Let's check if the class has an extractor attribute already
            ExtractingStockDeviceAttribute? classExtractorAttr = classType.GetCustomAttribute<ExtractingStockDeviceAttribute>();

            // Get all public const/static fields for that class
            var fields = classType.GetFields(BindingFlags.Static | BindingFlags.Public);

            Log.Debug("Starting foreach in fields...");
            foreach (var field in fields)
            {
                Log.Debug(@"Field: {@Field}", field.Name);

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

                Log.Debug(@"Getting extractor by DeviceExtractorType: {@DeviceExtractorType}", extractorAttr.DeviceExtractorType.FullName);

                var extractor = (IStockDeviceExtractor)svcProvider.GetRequiredService(extractorAttr.DeviceExtractorType);

                Log.Debug(@"Got extractor: {@Extractor}", extractor.GetType().FullName);

                dic.Add(key, extractor);
            }
        }

        return dic;
    }
}
