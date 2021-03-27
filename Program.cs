using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.XPath;

namespace AlsTools
{
    class Program
    {
        static AlsToolsManager manager = new AlsToolsManager();

        static async Task<int> Main(string[] args)
        {
            var arguments = GetArguments(args);
            if (!ValidateArguments(arguments))
                return -1;

            await PrintArguments(arguments);

            manager.Initialize(arguments);

            if (arguments.InteractiveMode)
            {
                int? option = null;
                while (option != 3)
                {
                    PrintMenu();
                    option = GetOption();
                    if (!option.HasValue)
                        continue;

                    if (option == 1)
                        arguments.ListPlugins = true;
                    else if (option == 2)
                        arguments.LocatePlugins = true;
                }
            }
            else
            {
                await manager.Execute(arguments);
            }

            await Console.Out.WriteLineAsync("------------------------------------------------------------------------------");
            await Console.Out.WriteLineAsync("DONE");

            return 0;
        }

        private static int? GetOption()
        {
            var opt = Console.Read();   //TODO: não está funcionando
            var validOpts = new int[] {1, 2, 3};
            
            if (!validOpts.Contains(opt))
            {
                Console.Error.WriteLine("\tInvalid option. Try again.");
                return null;
            }

            return opt;
        }

        private static void PrintMenu()
        {
            Console.WriteLine("\nSelect an option");
            Console.WriteLine("\t1 - List plugins");
            Console.WriteLine("\t2 - Locate plugins");
            Console.WriteLine("\t3 - Quit");
        }

        private static async Task PrintArguments(ProgramArgs args)
        {
            var text = new StringBuilder();

            text.AppendLine("ARGUMENTS: ");
            text.AppendLine($"Interactive mode: {args.InteractiveMode}");
            text.AppendLine($"Folder: {args.Folder}");
            text.AppendLine($"File: {args.File}");
            text.AppendLine($"List? {args.ListPlugins}");
            text.AppendLine($"Locate? {args.LocatePlugins}");
            text.AppendLine($"Plugins to locate: {string.Join("; ", args.PluginsToLocate)}");

            await Console.Out.WriteLineAsync(text);
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

            if (args.IndexOf("--list") >= 0)
                result.ListPlugins = true;

            if (args.IndexOf("--includebackups") >= 0)
                result.IncludeBackups = true;

            if (args.IndexOf("--interact") >= 0)
                result.InteractiveMode = true;

            int indexFolder = args.FindIndex(x => x.StartsWith("--folder="));
            if (indexFolder >= 0)
            {
                var parts = args[indexFolder].Split('=');
                if (parts.Count() == 2)
                    result.Folder = parts[1];
                else
                    Console.Error.WriteLine("Please specify a folder path!");
            }

            int indexFile = args.FindIndex(x => x.StartsWith("--file="));
            if (indexFile >= 0)
            {
                var parts = args[indexFile].Split('=');
                if (parts.Count() == 2)
                    result.File = parts[1];
                else
                    Console.Error.WriteLine("Please specify a file path!");
            }

            return result;
        }

        private static bool ValidateArguments(ProgramArgs args)
        {
            // Folder or file is always mandatory!
            if ((string.IsNullOrWhiteSpace(args.File) && string.IsNullOrWhiteSpace(args.Folder)) ||
                (!string.IsNullOrWhiteSpace(args.File) && !string.IsNullOrWhiteSpace(args.Folder)))
            {
                Console.Error.WriteLine("Please specify either a folder or file at least");
                return false;
            }

            if (args.InteractiveMode)
            {
                if (args.ListPlugins || args.LocatePlugins || args.PluginsToLocate.Any())
                {
                    Console.Error.WriteLine("In interactive mode, no other options can be used.");
                    return false;
                }
            }
            else // Non interactive mode
            {
                if ((args.ListPlugins && args.LocatePlugins) || (!args.ListPlugins && !args.LocatePlugins))
                {
                    Console.Error.WriteLine("Please specify either --list or --locate option");
                    return false;
                }
            }

            return true;
        }
    }
}
