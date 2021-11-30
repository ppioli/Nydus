#!/bin/bash
SCRIPT_DIR="$( cd -- "$( dirname -- "${BASH_SOURCE[0]}" )" &> /dev/null && pwd )"

VERSION="0.1.0"
#API_KEY="apikevalue"
source "${SCRIPT_DIR}/apikey.ini"
dotnet nuget push "../CoreKit.EntityHelper/bin/Debug/CoreKit.EntityHelper.${VERSION}.nupkg" --api-key "${API_KEY}" --source https://api.nuget.org/v3/index.json
