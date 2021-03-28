using System;
using System.Collections.Generic;
using System.Linq;
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
                int count = 0;
                if (!string.IsNullOrEmpty(args.File))
                    count = liveProjectService.InitializeDbFromFile(args.File);
                else
                    count = liveProjectService.InitializeDbFromFolder(args.Folder, args.IncludeBackups);

                await Console.Out.WriteLineAsync($"\nTotal of projects loaded into DB: {count}");
            }
            else if (args.CountProjects)
            {
                int count = liveProjectService.CountProjects();

                await Console.Out.WriteLineAsync($"\nTotal of projects in the DB: {count}");
            }
            else if (args.ListPlugins)
            {
                var projects = liveProjectService.GetAllProjects().ToList();
                await PrintProjectsAndPlugins(projects);
                await Console.Out.WriteLineAsync($"\nTotal of projects: {projects.Count}");
            }
            else if (args.LocatePlugins)
            {
                var projects = liveProjectService.GetProjectsContainingPlugins(args.PluginsToLocate).ToList();
                await PrintProjectsAndPlugins(projects);
                await Console.Out.WriteLineAsync($"\nTotal of projects: {projects.Count}");   
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
