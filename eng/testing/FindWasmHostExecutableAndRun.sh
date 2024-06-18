#!/usr/bin/env bash

set -e

# TODO: run .js and .mjs files using `node`

echo wasmtime run -S http "$@"
wasmtime run -S http "$@"
