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
                        var nav = xPathDoc.CreateNavigator();

                        GetProjectDetails(project, nav);
                        GetTracks(project, nav);
                        GetScenes(project, nav);
                        GetLocators(project, nav);
                    }
                }
            }

            return project;
        }


        //REFACTOR: refactor to a Locator Extractor
        private void GetLocators(LiveProject project, XPathNavigator nav)
        {
            var expression = $"/Ableton/LiveSet/Locators/Locators/Locator";
            var locatorsIterator = nav.Select(expression);

            foreach (XPathNavigator locatorNode in locatorsIterator)
            {
                var locator = new Locator()
                {
                    Number = locatorNode.SelectSingleNode(@"@Id")?.ValueAsInt,
                    Name = locatorNode.SelectSingleNode(@"Name/@Value")?.Value,
                    Annotation = locatorNode.SelectSingleNode(@"Annotation/@Value")?.Value,
                    Time = locatorNode.SelectSingleNode(@"Time/@Value")?.ValueAsInt,
                    IsSongStart = locatorNode.SelectSingleNode(@"IsSongStart/@Value")?.ValueAsBoolean
                };

                project.Locators.Add(locator);
            }
        }

        private void GetProjectDetails(LiveProject project, XPathNavigator nav)
        {
            project.Creator = GetProjectAttribute<string>(nav, "Creator");
            project.MajorVersion = GetProjectAttribute<string>(nav, "MajorVersion");
            project.MinorVersion = GetProjectAttribute<string>(nav, "MinorVersion");
            project.SchemaChangeCount = GetProjectAttribute<int>(nav, "SchemaChangeCount");
            project.Tempo = GetMasterTrackMixerAttribute<int>(nav, "Tempo");
            project.TimeSignature = GetMasterTrackMixerAttribute<int>(nav, "TimeSignature");
            project.GlobalGrooveAmount = GetMasterTrackMixerAttribute<int>(nav, "GlobalGrooveAmount");

        }

        private T GetMasterTrackMixerAttribute<T>(XPathNavigator nav, string attribute)
        {
            var expression = $"/Ableton/LiveSet/MasterTrack/DeviceChain/Mixer/{attribute}/Manual/@Value";
            return GetXpathValue<T>(nav, expression);
        }

        private T GetProjectAttribute<T>(XPathNavigator nav, string attribute)
        {
            var expression = $"/Ableton/@{attribute}";
            return GetXpathValue<T>(nav, expression);
        }

        private T GetXpathValue<T>(XPathNavigator nav, string expression)
        {
            var node = nav.Select(expression);

            if (!node.MoveNext())
                return default(T);

            var result = (T)node.Current.ValueAs(typeof(T));

            return result;
        }

        //REFACTOR: refactor to a Scene Extractor
        private void GetScenes(LiveProject project, XPathNavigator nav)
        {
            var expression = @"/Ableton/LiveSet/Scenes/Scene";
            var scenesIterator = nav.Select(expression);

            foreach (XPathNavigator sceneNode in scenesIterator)
            {
                var scene = new Scene()
                {
                    Number = sceneNode.SelectSingleNode(@"@Id")?.ValueAsInt,
                    Name = sceneNode.SelectSingleNode(@"Name/@Value")?.Value,
                    Annotation = sceneNode.SelectSingleNode(@"Annotation/@Value")?.Value,
                    Tempo = sceneNode.SelectSingleNode(@"Tempo/@Value")?.ValueAsInt,
                    IsTempoEnabled = sceneNode.SelectSingleNode(@"IsTempoEnabled/@Value")?.ValueAsBoolean,
                    TimeSignatureId = sceneNode.SelectSingleNode(@"TimeSignatureId/@Value")?.ValueAsInt,
                    IsTimeSignatureEnabled = sceneNode.SelectSingleNode(@"IsTimeSignatureEnabled/@Value")?.ValueAsBoolean
                };

                project.Scenes.Add(scene);
            }
        }

        //REFACTOR: refactor to a Track Extractor
        private void GetTracks(LiveProject project, XPathNavigator nav)
        {
            var expression = @"/Ableton/LiveSet/Tracks/AudioTrack";
            GetTrackByExpression(project, nav, expression, TrackType.Audio);

            expression = @"/Ableton/LiveSet/Tracks/MidiTrack";
            GetTrackByExpression(project, nav, expression, TrackType.Midi);

            expression = @"/Ableton/LiveSet/Tracks/ReturnTrack";
            GetTrackByExpression(project, nav, expression, TrackType.Return);

            expression = @"/Ableton/LiveSet/Tracks/GroupTrack";
            GetTrackByExpression(project, nav, expression, TrackType.Group);

            expression = @"/Ableton/LiveSet/MasterTrack";
            GetTrackByExpression(project, nav, expression, TrackType.Master);
        }

        private void GetTrackByExpression(LiveProject project, XPathNavigator nav, string expression, TrackType trackType)
        {
            var tracksIterator = nav.Select(expression);

            // Iterate through the tracks of the same type (audio, midi, return, master)
            foreach (XPathNavigator trackNode in tracksIterator)
            {
                var effectiveName = trackNode.SelectSingleNode(@"Name/EffectiveName/@Value")?.Value;
                var userName = trackNode.SelectSingleNode(@"Name/UserName/@Value")?.Value;
                var annotation = trackNode.SelectSingleNode(@"Name/Annotation/@Value")?.Value;
                var isFrozen = trackNode.SelectSingleNode(@"Freeze/@Value")?.ValueAsBoolean;
                var groupId = trackNode.SelectSingleNode(@"TrackGroupId/@Value")?.ValueAsInt;
                var trackDelay = new TrackDelay()
                {
                    Value = trackNode.SelectSingleNode(@"TrackDelay/Value/@Value")?.ValueAsInt,
                    IsValueSampleBased = trackNode.SelectSingleNode(@"TrackDelay/IsValueSampleBased/@Value")?.ValueAsBoolean
                };

                // Create the track
                var track = TrackFactory.CreateTrack(trackType, effectiveName, userName, annotation, isFrozen, trackDelay, groupId);

                // Get all children devices
                var devicesIterator = trackNode.Select(@"DeviceChain/DeviceChain/Devices");
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

        //REFACTOR: refactor to a Device Extractor
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


        //REFACTOR: refactor to a Plugin Extractor
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
