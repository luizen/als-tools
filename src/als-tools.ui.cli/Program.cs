namespace AlsTools.Ui.Cli;

public partial class Program
{
    private static readonly LoggingLevelSwitch levelSwitch = new LoggingLevelSwitch();

    public static async Task<int> Main(string[] args)
    {
        var parserResult = new Parser(config =>
        {
            config.CaseInsensitiveEnumValues = true;
            config.HelpWriter = Console.Error;
        }).ParseArguments<InitDbOptions, CountOptions, ListOptions, LocateOptions>(args);

        if (parserResult.Tag == ParserResultType.NotParsed)
        {
            await Console.Out.WriteLineAsync($"Command parsing error");
            Log.Debug("Returning {ReturnCode}", ProgramReturnCodes.CommandParseError);
            return ProgramReturnCodes.CommandParseError;
        }

        SetupLogging(parserResult);

        var host = BuildHost(args);

        using (var serviceScope = host.Services.CreateScope())
        {
            var services = serviceScope.ServiceProvider;

            try
            {
                var embeddedDbContext = services.GetRequiredService<IEmbeddedDatabaseContext>();
                embeddedDbContext.Initialize();

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
}
