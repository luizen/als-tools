using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Xml;
using System.Xml.XPath;
using AlsTools;

namespace AlsTools
{
    public class AlsToolsManager
    {
        public void Run(ProgramArgs arguments)
        {
            var d = new DirectoryInfo(arguments.Folder);
            var files = d.GetFiles("*.als", new EnumerationOptions() { RecurseSubdirectories = true });

            foreach (var f in files)
            {
                var project = ExtractLiveProjectInfoFromFile(f);
                PrintProjectAndPlugins(project);
            }

        }
        
        public void PrintProjectAndPlugins(LiveProject project)
        {
            Console.WriteLine("------------------------------------------------------------------------------");
            Console.WriteLine("Project name: {0}", project.Name);
            Console.WriteLine("Full path: {0}", project.Path);
            Console.WriteLine("\tPlugins:");

            if (project.Plugins.Count == 0)
            {
                Console.WriteLine("\t\tNo plugins found!");
                return;
            }

            foreach (var plugin in project.Plugins)
                Console.WriteLine("\t\tName = {0} | Type = {1}", plugin.Value.Name, plugin.Value.Type);
        }

        public LiveProject ExtractLiveProjectInfoFromFile(FileInfo fileToDecompress)
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

        public void GetPluginsByExpression(LiveProject project, XPathNavigator nav, string expression, PluginType type)
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
