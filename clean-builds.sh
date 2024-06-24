#!/bin/bash

echo "Cleaning all builds..."

dotnet clean src/als-tools.sln
rm -Rf -v src/als-tools.core/bin src/als-tools.core/obj
rm -Rf -v src/als-tools.infrastructure/bin src/als-tools.infrastructure/obj
rm -Rf -v src/als-tools.ui.cli/bin src/als-tools.ui.cli/obj
rm -Rf -v src/als-tools.ui.web/bin src/als-tools.ui.web/obj

echo "DONE"
