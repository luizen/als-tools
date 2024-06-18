# Ableton Live Set tools

## Introduction

Ableton Live Set tools, or simply als-tools, brings easy-to-use search, listing, counting and many other capabilities over your Ableton Live Set files (*.als).

> Notice: from now on, the term **project** will be used in place of **Ableton Live Set**, just for simplicity.

## Current features

- Scan multiple folders for *.als files, extract data from these files and save them in a database.
- List all projects stored in the database.
- Count the number of projects stored in the database.
- Locate projects containing one or more plugins, by plugin name (CLI only).
- CLI and [Web front-end](https://github.com/luizen/als-tools/issues/14).
- [Statistics](https://github.com/luizen/als-tools/issues/11)

## Future enhancements

Some of the most exciting future enhancements are:

- 💥 **[Interactive command line](https://github.com/luizen/als-tools/issues/7)**: navigate the options using a textual menu directly in your terminal! Take a look at [this project](https://github.com/shibayan/Sharprompt) for examples.
- 💥 ****: statistics like the number of tracks per project, most used devices/plugins, projects with most number of tracks, etc. Not only text/tables, but also nice charts.
- 💥 **[Powerful search/filtering](https://github.com/luizen/als-tools/issues/10)**: it will be possible to locate and filter projects by several properties, like plugin names, plugin types, device names, track names, track types, track count, etc.

For the complete (and ever growing) list of planned, future enhancements, please visit the [Issues](https://github.com/luizen/als-tools/issues) page.

## Building

DOCUMENTATION IS BEING REVIEWED AND UPDATED. WILL BE PUBLISHED VERY SOON ;)

## Running

DOCUMENTATION IS BEING REVIEWED AND UPDATED. WILL BE PUBLISHED VERY SOON ;)

## System requirements

### Operating System

Since not all music producers (Ableton Live users) work in/with the same platform / operating system, `als-tools` is being developed to be **cross-platform**, meaning it can be run on

- **Windows**
- **macOS**
- **Linux** (even though Ableton Live does not run on Linux)

### Development dependencies

The `als-tools` project is currently developed using [.NET 8](https://dotnet.microsoft.com). To build and run the tool from its source-code, the **.NET 8 SDK** must be installed on your computer. This includes the SDK itself and Runtime packages as well.

> **In a near future, ready-to-run binaries will be available in the releases page.**

Click [here](https://dotnet.microsoft.com/en-us/download) to visit the .NET download page.

## Tech stack

- [.NET 8](https://dotnet.microsoft.com) console application
- [Blazor server](https://dotnet.microsoft.com/en-us/apps/aspnet/web-apps/blazor) front-end using C#
- [Radzen](http://blazor.radzen.com) Blazor components
- [RavenDB](http://ravendb.net) embedded NoSQL document database
- [Serilog](http://serilog.net) logging library
- [CommandLineParser](https://github.com/commandlineparser/commandline) CLI parameters parsing
