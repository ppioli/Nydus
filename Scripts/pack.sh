#!/bin/bash
set -e
set -o pipefail

SCRIPT_DIR="$( cd -- "$( dirname -- "${BASH_SOURCE[0]}" )" &> /dev/null && pwd )"
VERSION="$1" 
cd "${SCRIPT_DIR}/../" || exit 1;
dotnet build
dotnet pack "${SCRIPT_DIR}/../src/Nydus.EntityHelper" /p:PackageVersion="${VERSION}" /p:Version="${VERSION}" -o "${SCRIPT_DIR}/../bin"
dotnet pack "${SCRIPT_DIR}/../src/Nydus.Fop" /p:PackageVersion="${VERSION}" /p:Version="${VERSION}" -o "${SCRIPT_DIR}/../bin"
dotnet pack "${SCRIPT_DIR}/../src/Nydus.Fop.Annotations" /p:PackageVersion="${VERSION}" /p:Version="${VERSION}" -o "${SCRIPT_DIR}/../bin"
dotnet pack "${SCRIPT_DIR}/../src/Nydus.Fop.Swashbuckle" /p:PackageVersion="${VERSION}" /p:Version="${VERSION}" -o "${SCRIPT_DIR}/../bin"

exit 0;
