using Radzen;

namespace als_tools.ui.web.Extensions;

public static class BuilderExtensions
{
    public static void ConfigureServices(this WebApplicationBuilder builder)
    {
        Log.Debug("Configuring builder.Services...");

        // Build configuration
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetParent(AppContext.BaseDirectory)!.FullName)
            .AddJsonFile($"appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
            .Build();

        // Add support to logging with Serilog, while removing all other logging providers
        builder.Logging.ClearProviders();
        builder.Host.UseSerilog((hostingContext, loggerConfiguration) =>
        {
            loggerConfiguration.ReadFrom.Configuration(configuration);
        });

        // Add Razor components
        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();

        // Add access to generic IConfigurationRoot
        builder.Services.AddSingleton<IConfigurationRoot>(configuration);

        // Add DbContext
        builder.Services.AddSingleton<EmbeddedDatabaseContext>();

        // Add some helpers
        builder.Services.AddSingleton<UserFolderHandler>(svcProvider =>
            new UserFolderHandler(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile, Environment.SpecialFolderOption.None)));

        builder.Services.AddSingleton<XpathExtractorHelper>();

        // MaxForLive Sort Extractors
        builder.Services.AddSingleton<IDictionary<string, IMaxForLiveDeviceSortExtractor>>(svcProvider =>
                     new Dictionary<string, IMaxForLiveDeviceSortExtractor>()
                     {
                         [DeviceTypeNodeName.MaxForLiveAudioEffect] = new MaxForLiveAudioEffectDeviceSortExtractor(svcProvider.GetRequiredService<ILogger<MaxForLiveAudioEffectDeviceSortExtractor>>()),
                         [DeviceTypeNodeName.MaxForLiveInstrument] = new MaxForLiveMidiInstrumentDeviceSortExtractor(svcProvider.GetRequiredService<ILogger<MaxForLiveMidiInstrumentDeviceSortExtractor>>()),
                         [DeviceTypeNodeName.MaxForLiveMidiEffect] = new MaxForLiveMidiEffectDeviceSortExtractor(svcProvider.GetRequiredService<ILogger<MaxForLiveMidiEffectDeviceSortExtractor>>())
                     }
                );

        // Plugin Format Extractors
        builder.Services.AddSingleton<IDictionary<PluginFormat, IPluginFormatExtractor>>(svcProvider =>
                     new Dictionary<PluginFormat, IPluginFormatExtractor>()
                     {
                         [PluginFormat.VST2] = new Vst2PluginFormatExtractor(svcProvider.GetRequiredService<ILogger<Vst2PluginFormatExtractor>>()),
                         [PluginFormat.VST3] = new Vst3PluginFormatExtractor(svcProvider.GetRequiredService<ILogger<Vst3PluginFormatExtractor>>()),
                         [PluginFormat.AUv2] = new AuV2PluginFormatExtractor(svcProvider.GetRequiredService<ILogger<AuV2PluginFormatExtractor>>())//,
                                                                                                                                                  //  [PluginFormat.AUv3] = new AuV3PluginFormatExtractor(svcProvider.GetRequiredService<ILogger<AuV3PluginFormatExtractor>>())
                     }
                );

        // Device Types by Node desc
        builder.Services.AddSingleton<IDictionary<string, DeviceType>>(svcProvider =>
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
        builder.Services.AddSingleton<ICommonStockAudioEffectDeviceExtractor, CommonStockAudioEffectDeviceExtractor>();
        builder.Services.AddSingleton<ICommonStockMidiEffectDeviceExtractor, CommonStockMidiEffectDeviceExtractor>();
        builder.Services.AddSingleton<ICommonStockInstrumentDeviceExtractor, CommonStockInstrumentDeviceExtractor>();

        // Live Stock Racks extractors
        builder.Services.AddSingleton<AudioEffectRackDeviceExtractor>();
        builder.Services.AddSingleton<MidiEffectRackDeviceExtractor>();
        builder.Services.AddSingleton<MidiInstrumentRackDeviceExtractor>();
        builder.Services.AddSingleton<DrumRackDeviceExtractor>();

        // All Live Stock device extractors by their XML node names
        builder.Services.AddSingleton<IDictionary<string, IStockDeviceExtractor>>(svcProvider => BuildStockDeviceExtractors(svcProvider));

        // Device type extractors
        builder.Services.AddSingleton(svcProvider =>
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
        builder.Services
            .AddSingleton<ILiveProjectAsyncService, LiveProjectAsyncService>()
            .AddSingleton<ILiveProjectAsyncRepository, LiveProjectRavenRepository>()
            .AddSingleton<ILiveProjectFileExtractionHandler, LiveProjectFileExtractionHandler>()
            .AddSingleton<ILiveProjectFileSystem, LiveProjectFileSystem>()
            .AddSingleton<ILiveProjectsCollectionExtractor, LiveProjectsCollectionExtractor>()
            .AddSingleton<IDevicesCollectionExtractor, DevicesCollectionExtractor>()
            .AddSingleton<ILocatorsCollectionExtractor, LocatorsCollectionExtractor>()
            .AddSingleton<IScenesCollectionExtractor, ScenesCollectionExtractor>()
            .AddSingleton<ITracksCollectionExtractor, TracksCollectionExtractor>();


        // Add Radzen UI components
        builder.Services.AddRadzenComponents(); ;

        // DB options
        builder.Services.Configure<DbOptions>(configuration.GetSection(nameof(DbOptions)));

        // InitDB options
        builder.Services.Configure<InitDbOptions>(configuration.GetSection(nameof(InitDbOptions)));

        // PlugInfo options
        builder.Services.Configure<PlugInfoOptions>(configuration.GetSection(nameof(PlugInfoOptions)));

        // PlugScanning options
        builder.Services.Configure<PlugScanningOptions>(configuration.GetSection(nameof(PlugScanningOptions)));
    }

    private static Dictionary<string, IStockDeviceExtractor> BuildStockDeviceExtractors(IServiceProvider svcProvider)
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

