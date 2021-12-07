#!/bin/bash
SCRIPT_DIR="$( cd -- "$( dirname -- "${BASH_SOURCE[0]}" )" &> /dev/null && pwd )"
VERSION="0.1.9"
#API_KEY="apikevalue"
source "${SCRIPT_DIR}/apikey.ini"

bash "${SCRIPT_DIR}/pack.sh" "${VERSION}"

dotnet nuget push "${SCRIPT_DIR}/../bin/Nydus.EntityHelper.${VERSION}.nupkg" --api-key "${API_KEY}" --source https://api.nuget.org/v3/index.json
dotnet nuget push "${SCRIPT_DIR}/../bin/Nydus.Fop.${VERSION}.nupkg" --api-key "${API_KEY}" --source https://api.nuget.org/v3/index.json
dotnet nuget push "${SCRIPT_DIR}/../bin/Nydus.Fop.Annotations.${VERSION}.nupkg" --api-key "${API_KEY}" --source https://api.nuget.org/v3/index.json
dotnet nuget push "${SCRIPT_DIR}/../bin/Nydus.Fop.Swashbuckle.${VERSION}.nupkg" --api-key "${API_KEY}" --source https://api.nuget.org/v3/index.json
