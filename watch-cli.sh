#!/bin/bash

echo "Watching CLI project..."

dotnet watch --project src/als-tools.ui.cli/als-tools.ui.cli.csproj "$@"
