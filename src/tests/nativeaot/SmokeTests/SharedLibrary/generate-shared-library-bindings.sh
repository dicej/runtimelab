#!/bin/sh

set -ex

# This script will regenerate the `wit-bindgen`-generated files in this
# directory.

# Prerequisites:
#   POSIX shell
#   tar
#   [cargo](https://rustup.rs/)
#   [curl](https://curl.se/download.html)

cargo install --locked --no-default-features --features csharp --version 0.27.0 wit-bindgen-cli
wit-bindgen c-sharp -w library -r native-aot wit
rm -r LibraryWorld_wasm_import_linkage_attribute.cs LibraryWorld_cabi_realloc.c
