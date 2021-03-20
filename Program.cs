using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Xml;
using System.Xml.XPath;

namespace AlsTools
{
    class Program
    {
        static int Main(string[] args)
        {
            var arguments = GetArguments(args);
            if (arguments == null)
                return -1;

            PrintArguments(arguments);

            var d = new DirectoryInfo(arguments.Folder);
            var files = d.GetFiles("*.als", new EnumerationOptions() { RecurseSubdirectories = true });

            foreach (var f in files)
            {
                var project = Decompress(f);
                PrintProjectAndPlugins(project);
            }

            Console.WriteLine("------------------------------------------------------------------------------");
            Console.WriteLine("DONE");

            return 0;
        }

        private static void PrintArguments(ProgramArgs args)
        {
            Console.WriteLine("ARGUMENTS: ");
            Console.WriteLine("Folder: {0}", args.Folder);
            Console.WriteLine("File: {0}", args.File);
            Console.WriteLine("List? {0}", args.ListPlugins);
            Console.WriteLine("Locate? {0}", args.LocatePlugins);
            Console.WriteLine("Plugins to locate: {0}", string.Join("; ", args.PluginsToLocate));
            
        }

        private static ProgramArgs GetArguments(string[] arguments)
        {
            var result = new ProgramArgs();
            var args = arguments.ToList();

            int indexLocate = args.FindIndex(x => x.StartsWith("--locate="));
            if (indexLocate >= 0)
            {
                var parts = args[indexLocate].Split('=');
                if (parts.Count() == 2)
                {
                    result.LocatePlugins = true;
                    result.PluginsToLocate = parts[1].Split(';');
                }                    
                else
                    Console.Error.WriteLine("Please specify a semicolon separated list of plugin names to locate!");
            }
            else if (args.IndexOf("--list") >= 0)
                result.ListPlugins = true;
            else
            {
                Console.Error.WriteLine("Please specify either --list or --locate option");
                return null;
            }

            int indexFolder = args.FindIndex(x => x.StartsWith("--folder="));
            if (indexFolder >= 0)
            {
                var parts = args[indexFolder].Split('=');
                if (parts.Count() == 2)
                    result.Folder = parts[1];
                else
                    Console.Error.WriteLine("Please specify a folder path!");
            }
            else
            {
                int indexFile = args.FindIndex(x => x.StartsWith("--file="));
                if (indexFile >= 0)
                {
                    var parts = args[indexFile].Split('=');
                    if (parts.Count() == 2)
                        result.File = parts[1];
                    else
                        Console.Error.WriteLine("Please specify a file path!");
                }                
            }

            return result;
        }

        public static void PrintProjectAndPlugins(LiveProject project)
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

        public static LiveProject Decompress(FileInfo fileToDecompress)
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

        public static void GetPluginsByExpression(LiveProject project, XPathNavigator nav, string expression, PluginType type)
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
