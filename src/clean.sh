#!/bin/bash

echo "Cleaning all..."

dotnet clean als-tools.sln
rm -Rf -v als-tools.core/bin als-tools.core/obj
rm -Rf -v als-tools.infrastructure/bin als-tools.infrastructure/obj
rm -Rf -v als-tools.ui.cli/bin als-tools.ui.cli/obj
rm -Rf -v als-tools.ui.web/bin als-tools.ui.web/obj

echo "DONE"
