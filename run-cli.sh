#!/bin/bash

echo "Running CLI project..."

dotnet run --project src/als-tools.ui.cli/als-tools.ui.cli.csproj "$@"
