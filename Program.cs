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
        static AlsToolsManager manager = new AlsToolsManager();

        static int Main(string[] args)
        {
            var arguments = GetArguments(args);
            if (arguments == null)
                return -1;

            PrintArguments(arguments);

            manager.Run(arguments);

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

        
    }
}
