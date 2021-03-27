using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Xml;
using System.Xml.XPath;
using AlsTools.Core.Entities;
using AlsTools.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace AlsTools.Infrastructure
{
    public class LiveProjectExtractor : ILiveProjectExtractor
    {
        ILogger<LiveProjectExtractor> logger;

        public LiveProjectExtractor(ILogger<LiveProjectExtractor> logger)
        {
            this.logger = logger;
        }

        public LiveProject ExtractProjectFromFile(FileInfo file)
        {
            logger.LogDebug("Extracting file {file}", file.Name);

            var project = new LiveProject() { Name = file.Name, Path = file.FullName };
            var plugins = new SortedSet<string>();
            var settings = new XmlReaderSettings() { IgnoreWhitespace = true };

            using (FileStream originalFileStream = file.OpenRead())
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
