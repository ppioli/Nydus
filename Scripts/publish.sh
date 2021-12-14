#!/bin/bash
SCRIPT_DIR="$( cd -- "$( dirname -- "${BASH_SOURCE[0]}" )" &> /dev/null && pwd )"

if [[ -z "$1" ]]; then 
    echo "Error: Must provide the version number"
    exit 1
fi

#API_KEY="apikevalue"
source "${SCRIPT_DIR}/apikey.ini"
if [[ -z "$API_KEY" ]]; then 
    echo "Error: Could not load the api key. Define API_KEY='...' on ./apikey file"
    exit 1
fi


dotnet nuget push "${SCRIPT_DIR}/../bin/Nydus.EntityHelper.${1}.nupkg" --api-key "${API_KEY}" --source https://api.nuget.org/v3/index.json
dotnet nuget push "${SCRIPT_DIR}/../bin/Nydus.Fop.${1}.nupkg" --api-key "${API_KEY}" --source https://api.nuget.org/v3/index.json
dotnet nuget push "${SCRIPT_DIR}/../bin/Nydus.Fop.Annotations.${1}.nupkg" --api-key "${API_KEY}" --source https://api.nuget.org/v3/index.json
dotnet nuget push "${SCRIPT_DIR}/../bin/Nydus.Fop.Swashbuckle.${1}.nupkg" --api-key "${API_KEY}" --source https://api.nuget.org/v3/index.json
