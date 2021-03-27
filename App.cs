using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.XPath;
using AlsTools.Core.Entities;
using AlsTools.Core.Interfaces;

namespace AlsTools
{
    public class App
    {
        // private readonly ILogger<App> _logger;
        // private readonly AppSettings _appSettings;
        private readonly ILiveProjectService liveProjectService;
        
        public App(ILiveProjectService liveProjectService)
        {
            this.liveProjectService = liveProjectService;
        }

        public async Task Run(ProgramArgs args)
        {
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

        private async Task PrintProjectsAndPlugins(IList<LiveProject> projects)
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
