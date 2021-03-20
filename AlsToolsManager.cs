using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.XPath;
using AlsTools;

namespace AlsTools
{
    public class AlsToolsManager
    {
        IList<LiveProject> projects = new List<LiveProject>();

        public void Initialize(ProgramArgs arguments)
        {
            var d = new DirectoryInfo(arguments.Folder);
            var files = d.GetFiles("*.als", new EnumerationOptions() { RecurseSubdirectories = true });

            foreach (var f in files)
            {
                var project = ExtractLiveProjectInfoFromFile(f);
                projects.Add(project);
            }
        }

        public async Task Execute(ProgramArgs arguments)
        {
            foreach (var project in projects)
            {
                await PrintProjectAndPlugins(project);
            }
        }

        private async Task<bool> PrintProjectAndPlugins(LiveProject project)
        {
            await Console.Out.WriteLineAsync("------------------------------------------------------------------------------");
            await Console.Out.WriteLineAsync($"Project name: {project.Name}");
            await Console.Out.WriteLineAsync($"Full path: {project.Path}");
            await Console.Out.WriteLineAsync("\tPlugins:");

            if (project.Plugins.Count == 0)
            {
                await Console.Out.WriteLineAsync("\t\tNo plugins found!");
                return false;
            }

            foreach (var plugin in project.Plugins)
                await Console.Out.WriteLineAsync($"\t\tName = {plugin.Value.Name} | Type = {plugin.Value.Type}");

            return true;
        }

        private LiveProject ExtractLiveProjectInfoFromFile(FileInfo fileToDecompress)
        {
            var project = new LiveProject() { Name = fileToDecompress.Name, Path = fileToDecompress.FullName };
            var plugins = new SortedSet<string>();
            var settings = new XmlReaderSettings() { IgnoreWhitespace = true };

            using (FileStream originalFileStream = fileToDecompress.OpenRead())
            {
                using (GZipStream decompressionStream = new GZipStream(originalFileStream, CompressionMode.Decompress))
                {
                    using (StreamReader unzip = new StreamReader(decompressionStream))
                    {
                        var xPathDoc = new XPathDocument(unzip);
                        var nav = xPathDoc.CreateNavigator();

                        var expression = @"//PluginDevice/PluginDesc/Vst3PluginInfo/Name/@Value";
                        GetPluginsByExpression(project, nav, expression, PluginType.VST3);

                        expression = @"//PluginDevice/PluginDesc/VstPluginInfo/PlugName/@Value";
                        GetPluginsByExpression(project, nav, expression, PluginType.VST);

                        expression = @"//AuPluginDevice/PluginDesc/AuPluginInfo/Name/@Value";
                        GetPluginsByExpression(project, nav, expression, PluginType.AU);
                    }
                }
            }

            return project;
        }

        private void GetPluginsByExpression(LiveProject project, XPathNavigator nav, string expression, PluginType type)
        {
            var nodeIter = nav.Select(expression);

            while (nodeIter.MoveNext())
            {
                var name = nodeIter.Current.Value;
                if (!project.Plugins.ContainsKey(name))
                {
                    var p = new PluginInfo() { Name = name, Type = type };
                    project.Plugins.Add(p.Name, p);
                }
            }
        }
    }
}
