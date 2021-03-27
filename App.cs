using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AlsTools.Core.Entities;
using AlsTools.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace AlsTools
{
    public class App
    {
        private readonly ILogger<App> logger;
        private readonly ILiveProjectService liveProjectService;
        
        public App(ILogger<App> logger, ILiveProjectService liveProjectService)
        {
            this.logger = logger;
            this.liveProjectService = liveProjectService;
        }

        public async Task Run(ProgramArgs args)
        {
            logger.LogDebug("App start");

            if (args.InitDb)
            {
                if (!string.IsNullOrEmpty(args.File))
                    liveProjectService.InitializeDbFromFile(args.File);
                else
                    liveProjectService.InitializeDbFromFolder(args.Folder, args.IncludeBackups);
            }
            else if (args.ListPlugins)
            {
                var projects = liveProjectService.GetAllProjects();
                await PrintProjectsAndPlugins(projects);
            }
            else if (args.LocatePlugins)
            {
                var projects = liveProjectService.GetProjectsContainingPlugins(args.PluginsToLocate);
                await PrintProjectsAndPlugins(projects);
            }
            else
            {
                throw new InvalidOperationException("Nothing to do?");
            }
        }

        private async Task PrintProjectsAndPlugins(IEnumerable<LiveProject> projects)
        {
            foreach (var p in projects)
                await PrintProjectAndPlugins(p);
        }

        private async Task PrintProjectAndPlugins(LiveProject project)
        {
            await Console.Out.WriteLineAsync("------------------------------------------------------------------------------");
            await Console.Out.WriteLineAsync($"Project name: {project.Name}");
            await Console.Out.WriteLineAsync($"Full path: {project.Path}");
            await Console.Out.WriteLineAsync("\tPlugins:");

            if (project.Plugins.Count == 0)
                await Console.Out.WriteLineAsync("\t\tNo plugins found!");

            foreach (var plugin in project.Plugins)
                await Console.Out.WriteLineAsync($"\t\tName = {plugin.Value.Name} | Type = {plugin.Value.Type}");
        }
    }
}
