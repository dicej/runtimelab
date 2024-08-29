#!/bin/sh

set -ex

# This script will regenerate the `wit-bindgen`-generated files in this
# directory.

# Prerequisites:
#   POSIX shell
#   tar
#   [cargo](https://rustup.rs/)
#   [curl](https://curl.se/download.html)

# TODO: switch to crates.io release once https://github.com/bytecodealliance/wit-bindgen/pull/1040 is merged and released
cargo install --locked --no-default-features --features csharp --git https://github.com/dicej/wit-bindgen --rev 34afca03 wit-bindgen-cli
curl -OL https://github.com/WebAssembly/wasi-http/archive/refs/tags/v0.2.1.tar.gz
tar xzf v0.2.1.tar.gz
cat >wasi-http-0.2.1/wit/world.wit <<EOF
world wasi-poll {
  import wasi:io/poll@0.2.1;
  import wasi:clocks/monotonic-clock@0.2.1;
}
EOF
wit-bindgen c-sharp -w wasi-poll -r native-aot --internal --skip-support-files wasi-http-0.2.1/wit
rm -r wasi-http-0.2.1 v0.2.1.tar.gz
