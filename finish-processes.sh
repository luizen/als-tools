#!/bin/bash

echo "Killing dotnet and ravendb processes..."
echo $(ps aux | grep -i -E '[a]ls-tools.dll|[r]aven.server.dll' | awk '{print $2}')
ps aux | grep -i -E '[a]ls-tools.dll|[r]aven.server.dll' | awk '{print $2}' | xargs kill
echo "DONE"