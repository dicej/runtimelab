#!/usr/bin/env bash

# This script is a bridge that allows running NativeAOT compiled executables instead of using corerun.
#
# To use this script, set the CLRCustomTestLauncher environment variable to the full path of this script.
#
# The .cmd files of the individual tests will call this script to launch the test.
# This script gets the following arguments
# 1. Full path to the directory of the test binaries (the test .sh file is in there)
# 2. Filename of the test executable
# 3. - n. Additional arguments that were passed to the test .sh

SCRIPT_DIR=$(cd -- "$(dirname -- "${BASH_SOURCE[0]}")" &> /dev/null && pwd)

source $SCRIPT_DIR/../../../../eng/testing/FindWasmHostExecutable.sh "$1/native/$2"

if [ -n "${WASM_HOST_EXECUTABLE}" ]; then
  echo $WASM_HOST_EXECUTABLE "$WASM_BINARY_TO_EXECUTE" "${@:3}"
  $WASM_HOST_EXECUTABLE "$WASM_BINARY_TO_EXECUTE" "${@:3}"
else
  chmod +x $1/native/$exename
  $_DebuggerFullPath $1/native/$exename "${@:3}"
fi
