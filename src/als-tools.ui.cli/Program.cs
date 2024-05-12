using AlsTools.Core.Services;
using AlsTools.Infrastructure.Models;
using Microsoft.Extensions.Hosting;

namespace AlsTools.Ui.Cli;

public partial class Program
{

    public static async Task Main(string[] args)
    {
        var builder = new HostBuilder()
            .ConfigureServices((hostContext, services) =>
            {
                services.AddLogging(configure => configure.AddConsole().SetMinimumLevel(LogLevel.Information))
                    .AddTransient<App>()
                    .AddTransient<ILiveProjectAsyncService, LiveProjectAsyncService>()
                    .AddTransient<ILiveProjectAsyncRepository, LiveProjectAsyncRepository>();

                services.AddDbContext<MyNewDbContext>();
            });

        var host = builder.Build();
        var app = host.Services.GetRequiredService<App>();

        await app.Run();
    }
}
