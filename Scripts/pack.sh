#!/bin/bash
set -e
set -o pipefail
# A POSIX variable
OPTIND=1 # Reset in case getopts has been used previously in the shell.

SCRIPT_DIR="$(cd -- "$(dirname -- "${BASH_SOURCE[0]}")" &>/dev/null && pwd)"
RELEASE=''

VERSION_PREFIX="$(cat ./version)"
export VERSION_PREFIX
BUILD_NUMBER="b+$(date '+%s')"
export BUILD_NUMBER



show_help() {
  echo "-h Show this messasge"
  echo "-r Release build"
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

dotnet pack -o "${SCRIPT_DIR}/../bin" "${SCRIPT_DIR}/../src/Nydus.EntityHelper" 
dotnet pack -o "${SCRIPT_DIR}/../bin" "${SCRIPT_DIR}/../src/Nydus.Fop"
dotnet pack -o "${SCRIPT_DIR}/../bin" "${SCRIPT_DIR}/../src/Nydus.Fop.Annotations"
dotnet pack -o "${SCRIPT_DIR}/../bin" "${SCRIPT_DIR}/../src/Nydus.Fop.Swashbuckle"
