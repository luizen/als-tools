using System.IO;
using System.IO.Compression;
using System.Xml.XPath;
using AlsTools.Core.Entities;
using AlsTools.Core.Factories;
using AlsTools.Core.Interfaces;
using AlsTools.Core.ValueObjects;
using AlsTools.Core.ValueObjects.Devices;
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
            logger.LogDebug("Extracting file {file}", file.FullName);

            var project = new LiveProject() { Name = file.Name, Path = file.FullName };

            using (FileStream originalFileStream = file.OpenRead())
            {
                using (GZipStream decompressionStream = new GZipStream(originalFileStream, CompressionMode.Decompress))
                {
                    using (StreamReader unzip = new StreamReader(decompressionStream))
                    {
                        var xPathDoc = new XPathDocument(unzip);

                        GetProjectDetails(project, xPathDoc);

                        GetTracks(project, xPathDoc);
                    }
                }
            }

            return project;
        }

        private void GetProjectDetails(LiveProject project, XPathDocument xPathDoc)
        {
            var nav = xPathDoc.CreateNavigator();
            var expression = @"/Ableton/@Creator";
            var creatorNode = nav.Select(expression);
            
            if (!creatorNode.MoveNext())
                return;

            project.Creator = creatorNode.Current.Value;
        }

        private void GetTracks(LiveProject project, XPathDocument xPathDoc)
        {
            var nav = xPathDoc.CreateNavigator();

            var expression = @"//LiveSet/Tracks/AudioTrack";
            GetTrackByExpression(project, nav, expression, TrackType.Audio);

            expression = @"//LiveSet/Tracks/MidiTrack";
            GetTrackByExpression(project, nav, expression, TrackType.Midi);

            expression = @"//LiveSet/Tracks/ReturnTrack";
            GetTrackByExpression(project, nav, expression, TrackType.Return);

            expression = @"//LiveSet/MasterTrack";
            GetTrackByExpression(project, nav, expression, TrackType.Master);
        }

        private void GetTrackByExpression(LiveProject project, XPathNavigator nav, string expression, TrackType trackType)
        {
            var tracksIterator = nav.Select(expression);

            // Iterate through the tracks of the same type (audio, midi, return, master)
            while (tracksIterator.MoveNext())
            {
                // Get track name
                var nameNode = tracksIterator.Current.Select(@"Name/EffectiveName/@Value");
                nameNode.MoveNext();

                // Create the track
                var track = TrackFactory.CreateTrack(trackType, nameNode.Current.Value);

                // Get all children devices
                var devicesIterator = tracksIterator.Current.Select(@"DeviceChain/DeviceChain/Devices");
                devicesIterator.MoveNext();
                if (devicesIterator.Current.HasChildren)
                {
                    if (devicesIterator.Current.MoveToFirstChild())
                    {
                        // Get first device
                        var deviceNode = devicesIterator.Current;
                        var device = DeviceFactory.CreateDeviceByNodeName(deviceNode.Name);

                        GetDeviceInformation(deviceNode, device);

                        // Add to devices list
                        track.AddDevice(device);

                        // Iterate through all other devices
                        while (devicesIterator.Current.MoveToNext())
                        {
                            deviceNode = devicesIterator.Current;
                            device = DeviceFactory.CreateDeviceByNodeName(deviceNode.Name);
                            GetDeviceInformation(deviceNode, device);
                            track.AddDevice(device);
                        }
                    }
                }

                project.Tracks.Add(track);
            }
        }

        private void GetDeviceInformation(XPathNavigator deviceNode, IDevice device)
        {
            if (device.Type == DeviceType.LiveDevice)
            {
                device.Name = deviceNode.Name;
                return;
            }
                

            var pluginDescNode = deviceNode.Select(@"PluginDesc");
            pluginDescNode.MoveNext();
            if (pluginDescNode.Current.HasChildren)
            {
                if (pluginDescNode.Current.MoveToFirstChild())
                {
                    var pluginInfo = GetPluginNameAndType(pluginDescNode.Current);
                    device.Name = pluginInfo.Item1;
                    (device as PluginDevice).PluginType = pluginInfo.Item2;
                }
            }
        }

        private (string, PluginType) GetPluginNameAndType(XPathNavigator pluginDescNode)
        {
            var pluginDescNodeName = pluginDescNode.Name.ToUpperInvariant();
            string pluginName = string.Empty;
            PluginType pluginType = PluginType.AU;
            XPathNodeIterator nodeIterator = null;

            switch (pluginDescNodeName)
            {
                case "VSTPLUGININFO":
                    nodeIterator = pluginDescNode.Select(@"PlugName/@Value");
                    nodeIterator.MoveNext();
                    pluginName = nodeIterator.Current.Value;
                    pluginType = PluginType.VST2;
                    break;

                case "VST3PLUGININFO":
                    nodeIterator = pluginDescNode.Select(@"Name/@Value");
                    nodeIterator.MoveNext();
                    pluginName = nodeIterator.Current.Value;
                    pluginType = PluginType.VST3;
                    break;

                default:
                    nodeIterator = pluginDescNode.Select(@"Name/@Value");
                    nodeIterator.MoveNext();
                    pluginName = nodeIterator.Current.Value;
                    pluginType = PluginType.AU;
                    break;
            }

            return (pluginName, pluginType);
        }
    }
}
