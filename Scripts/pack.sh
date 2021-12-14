#!/bin/bash
set -e
set -o pipefail
# A POSIX variable
OPTIND=1 # Reset in case getopts has been used previously in the shell.

SCRIPT_DIR="$(cd -- "$(dirname -- "${BASH_SOURCE[0]}")" &>/dev/null && pwd)"
RELEASE=''

VERSION_PREFIX="$(cat ./version)"
export VERSION_PREFIX
VERSION_SUFFIX="$(date '+%s')"
export VERSION_SUFFIX

echo "$VERSION_SUFFIX";

show_help() {
  echo "-h Show this message"
  echo "-r Build release version"
}

while getopts "h?r" opt; do
  case "$opt" in
  h | \?)
    show_help
    exit 0
    ;;
  r)
    RELEASE="1"
    ;;
  esac
done

shift $((OPTIND - 1))
[ "${1:-}" = "--" ] && shift

export RELEASE;

dotnet pack -o "${SCRIPT_DIR}/../bin" "${SCRIPT_DIR}/../src/Nydus.EntityHelper" 
dotnet pack -o "${SCRIPT_DIR}/../bin" "${SCRIPT_DIR}/../src/Nydus.Fop"
dotnet pack -o "${SCRIPT_DIR}/../bin" "${SCRIPT_DIR}/../src/Nydus.Fop.Annotations"
dotnet pack -o "${SCRIPT_DIR}/../bin" "${SCRIPT_DIR}/../src/Nydus.Fop.Swashbuckle"

if [[ "$RELEASE" == '1' ]]; then
  echo "Publishing"
  bash "${SCRIPT_DIR}/publish.sh" "${VERSION_PREFIX}"
fi
