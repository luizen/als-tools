namespace AlsTools.Ui.Cli.OptionCommandHandlers.Handlers;

public partial class PrintStatisticsCommandHandler : BaseCommandHandler, IOptionCommandHandler<PrintStatisticsOptions>
{
    private async Task ExecuteIfOptionWasSetAsync(bool optionValue, Func<Task> asyncAction)
    {
        if (optionValue)
            await asyncAction();
    }

    private void SetAllOptionsIfAll(PrintStatisticsOptions options)
    {
        if (options.All)
        {
            options.PluginsPerProject =
                options.TracksPerProject =
                    options.CountProjects =
                        options.StockDevicesPerProject =
                            options.MostUsedPlugins =
                                options.MostUsedStockDevice =
                                    options.ProjectsWithHighestPluginCount =
                                        options.ProjectsWithHighestTrackCount = true;
        }
    }
}