#!/bin/bash
SCRIPT_DIR="$( cd -- "$( dirname -- "${BASH_SOURCE[0]}" )" &> /dev/null && pwd )"
VERSION="0.1.1"
#API_KEY="apikevalue"
source "${SCRIPT_DIR}/apikey.ini"

if bash "${SCRIPT_DIR}/pack.sh" "${VERSION}"; then
  echo "Pack failed. Leaving..."
  exit 1
fi 

#dotnet nuget push "${SCRIPT_DIR}/../Nydus.EntityHelper/bin/Debug/Nydus.EntityHelper.${VERSION}.nupkg" "${OPTIONS}" 
#dotnet nuget push "${SCRIPT_DIR}/../Nydus.Fop/bin/Debug/Nydus.Fop.${VERSION}.nupkg" --api-key "${API_KEY}" "${OPTIONS}"
#dotnet nuget push "${SCRIPT_DIR}/../Nydus.Fop.Annotations/bin/Debug/Nydus.Fop.Annotations.${VERSION}.nupkg" "${OPTIONS}"
#dotnet nuget push "${SCRIPT_DIR}/../Nydus.Fop.Swashbuckle/bin/Debug/Nydus.Fop.Swashbuckle.${VERSION}.nupkg" "${OPTIONS}"
