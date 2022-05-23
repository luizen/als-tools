using System;
using System.Threading.Tasks;
using AlsTools.CliOptions;
using AlsTools.Core.Interfaces;
using CommandLine;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Core;

namespace AlsTools;

public partial class Program
{
    private static IConfigurationRoot configuration;
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
            Log.Debug("Returning {ReturnCode}", ProgramReturnCodes.Ok);
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
