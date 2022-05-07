# Ableton Live Set tools

## Introduction

Ableton Live Set tools, or simply als-tools, brings easy-to-use search, listing, counting and many other capabilities over your Ableton Live Set files (*.als).

> Notice: from now on, the term **project** will be used in place of **Ableton Live Set**, just for simplicity.

## Current features

- Scan multiple folders for *.als files, extract data from these files and save them in a database.
- List all projects stored in the database.
- Count the number of projects stored in the database.
- Locate projects containing one or more plugins, by plugin name.

## Future enhancements

Some of the most exciting future enhancements are:

- 💥 **[Web front-end](https://github.com/luizen/als-tools/issues/14)**: a beautiful and easy-to-use web front-end;
- 💥 **[Interactive command line](https://github.com/luizen/als-tools/issues/7)**: navigate the options using a textual menu directly in your terminal! Take a look at [this project](https://github.com/shibayan/Sharprompt) for examples.
- 💥 **[Statistics](https://github.com/luizen/als-tools/issues/11)**: statistics like the number of tracks per project, most used devices/plugins, projects with most number of tracks, etc. Not only text/tables, but also nice charts.
- 💥 **[Powerful search/filtering](https://github.com/luizen/als-tools/issues/10)**: it will be possible to locate and filter projects by several properties, like plugin names, plugin types, device names, track names, track types, track count, etc.

For the complete (and ever growing) list of planned, future enhancements, please visit the [Issues](https://github.com/luizen/als-tools/issues) page.

## Building

> This is a console application, which means all commands are required to be run in a command-line interpreter / terminal emulator. Some examples:
>
> - Windows: [Command Prompt (cmd.exe)](https://en.wikipedia.org/wiki/Cmd.exe), [PowerShell](https://en.wikipedia.org/wiki/PowerShell) or [Windows Terminal](https://en.wikipedia.org/wiki/Windows_Terminal);
> - macOS: [Terminal](https://en.wikipedia.org/wiki/Terminal_(macOS)) or other alternatives, like the great [iTerm2](https://iterm2.com);
> - Linux: [GNOME Terminal](https://en.wikipedia.org/wiki/GNOME_Terminal), [Konsole](https://en.wikipedia.org/wiki/Konsole);
>
> Click [here](https://en.wikipedia.org/wiki/List_of_terminal_emulators) for a list of terminal emulators/consoles.

```programming
dotnet build *.sln --configuration Release
```

## Running

> Right now there aren't any platform-specific full package, binary releases (Windows, macOS, Linux). So the way to run the tool is to download its source-code, build it and run it yourself. Check the [Development dependencies section](#development-dependencies) for more details. A [github issue (#21)](https://github.com/luizen/als-tools/issues/21) has already been created to handle this as a future enhancement.

1. Initialize the database with data extracted from Live sets files (*.als).

    Supposing your projects are under `/Users/myuser/Music/My Live Projects` and `/Users/myuser/Splice`, the following command ❗must be executed first❗ so that it **a)** scans all projects, extracting project information plus their details (currently: the tracks, plugins and devices) and **b)** loads them into the application database for further analysis.

    ```programming
    dotnet run initdb --folders "/Users/myuser/Music/My Live Projects" "/Users/myuser/Splice"
    ```

2. After the database is initialized, further commands can be executed.

- To count how many projects the database is loaded with:

    ```programming
    dotnet run count
    ```

- To list all projects, its plugins, devices and tracks:

    ```programming
    dotnet run list
    ```

- To locate all projects containing at least one of the plugins (_contains plugin name_):

    ```programming
    dotnet run locate --plugin-names plugin1 "plugin 2" [...]
    ```

    > Where **plugin1** and **"plugin 2"**, etc, should be the names of the plugins to locate projects using them.
    > Example:
    >
    > ```programming
    > dotnet run --plugin-names "Abbey Road Vinyl" HG-2 bx_solo
    > ```

## Help and commands (verbs)

To see all available command line verbs and options, just execute the following command:

```programming
dotnet run
```

Example of output:
```programming
als-tools 1.0.0
Copyright (C) 2022 als-tools

ERROR(S):
  No verb selected.

  initdb     Initialize the als-tools database with information extracted from Live sets, either from files or folders.

  count      Returns the total of projects stored in the als-tools database.

  list       List all projects stored in the als-tools database.

  locate     Locates projects containing given plugins by their names.

  help       Display more information on a specific command.

  version    Display version information.
```

## System requirements

### Operating System

Since not all music producers (Ableton Live users) work in/with the same platform / operating system, `als-tools` is being developed to be **cross-platform**, meaning it can be run on

- **Windows**
- **macOS**
- **Linux** (even though Ableton Live does not run on Linux)

### Development dependencies

The `als-tools` project is currently developed using [.NET 6](https://docs.microsoft.com/en-us/dotnet/). To build and run the tool from its source-code, the **.NET 6 SDK** must be installed on your computer. This includes the SDK itself and Runtime packages as well.

Click [here](https://dotnet.microsoft.com/en-us/download) to visit the .NET download page.

## Tech stack

- [.NET 6](https://docs.microsoft.com/en-us/dotnet/) console application
- [RavenDB](http://ravendb.net) - Embedded NoSQL document database
- [Serilog](http://serilog.net) - Logging library
- [CommandLineParser](https://github.com/commandlineparser/commandline) - CLI parameters parsing
