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
            else if (args.Export)
            {
                var projects = liveProjectService.GetAllProjects().ToList();
                await ExportProjectsAndPlugins(projects);
                await Console.Out.WriteLineAsync($"\nTotal of projects: {projects.Count}");
            }
            else
            {
                throw new InvalidOperationException("Nothing to do?");
            }
        }

        private Task ExportProjectsAndPlugins(List<LiveProject> projects)
        {
            throw new NotImplementedException();
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
            await Console.Out.WriteLineAsync("\tTracks and plugins:");
            
            if (project.Tracks.Count == 0)
                await Console.Out.WriteLineAsync("\t\tNo tracks found!");

            foreach (var tr in project.Tracks)
            {
                await Console.Out.WriteLineAsync($"\t\tName = {tr.Name} | Type = {tr.Type}");

                await Console.Out.WriteLineAsync("\t\t\tLive Devices:");
                foreach (var ld in tr.Devices)
                    await Console.Out.WriteLineAsync($"\t\t\t\tName = {ld.Key}");

                await Console.Out.WriteLineAsync("\t\t\tPlugins:");
                foreach (var p in tr.Plugins)
                    await Console.Out.WriteLineAsync($"\t\t\t\tName = {p.Key} | Type = {p.Value.PluginType}");
            }
        }
    }
}
