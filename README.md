# als-tools
Ableton Live Set tools

## Building
```
dotnet build *.sln --configuration Release
```

## Running

Supposing your Live sets are under `/Users/myuser/Music/Projects`, the following command **must be executed first** so that it reads all Live sets plus its plugins and loads them into the application database for further analysis.

```
dotnet run --no-build --initdb --folder=/Users/myuser/Music/Projects
```

After the database is initialized, you can execute further commands.

- To count how many projects there are in the database:
    ```
    dotnet run --no-build --count
    ```

- To list all Live projects and its plugins:
    ```
    dotnet run --no-build --list
    ```

- To locate all projects containing at least one of the plugins:
    ```
    dotnet run --no-build --locate=plugin1;plugin2 ...
    ```
    > Where *plugin1* and *plugin2*, etc, should be the names of the plugins to locate projects using them.
    > Example: dotnet run --no-build --locate="Abbey Road Vinyl";HG-2;bx_solo