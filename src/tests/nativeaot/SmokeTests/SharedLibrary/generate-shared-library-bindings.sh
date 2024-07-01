#!/bin/sh

set -ex

# This script will regenerate the `wit-bindgen`-generated files in this
# directory.

# Prerequisites:
#   POSIX shell
#   tar
#   [cargo](https://rustup.rs/)
#   [curl](https://curl.se/download.html)

# TODO: Use a crates.io release instead of the Git repo once a release
# containing this commit has been created:
cargo install --locked --no-default-features --features csharp-naot \
      --git https://github.com/bytecodealliance/wit-bindgen --rev 266d638f7a9c4535ba5fa1f1bb2e8cc6b5d58667 \
      wit-bindgen-cli
wit-bindgen c-sharp -w library -r native-aot wit
rm -r LibraryWorld_wasm_import_linkage_attribute.cs
