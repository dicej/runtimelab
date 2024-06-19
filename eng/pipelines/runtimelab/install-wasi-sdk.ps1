Invoke-WebRequest -Uri https://github.com/WebAssembly/wasi-sdk/releases/download/wasi-sdk-22/wasi-sdk-22.0.m-mingw64.tar.gz -OutFile wasi-sdk-22.0.m-mingw64.tar.gz

tar -xzf wasi-sdk-22.0.m-mingw64.tar.gz

mv wasi-sdk-22.0+m wasi-sdk

# Temporary WASI-SDK 22 workaround: Until
# https://github.com/WebAssembly/wasi-libc/issues/501 is addressed, we copy
# pthread.h from the wasm32-wasi-threads include directory to the wasm32-wasip2
# include directory.  See https://github.com/dotnet/runtimelab/issues/2598 for
# the issue to remove this workaround once WASI-SDK 23 is released.

cp wasi-sdk/share/wasi-sysroot/include/wasm32-wasi-threads/pthread.h wasi-sdk/share/wasi-sysroot/include/wasm32-wasip2/

# Temporary WASI-SDK 22 workaround #2: The version of `wasm-component-ld` that
# ships with WASI-SDK 22 contains a
# [bug](https://github.com/bytecodealliance/wasm-component-ld/issues/22) which
# has been fixed in a v0.5.3 of that utility, so we upgrade it here.

Invoke-WebRequest -Uri https://github.com/bytecodealliance/wasm-component-ld/releases/download/v0.5.3/wasm-component-ld-v0.5.3-x86_64-windows.zip -OutFile wasm-component-ld-v0.5.3-x86_64-windows.zip

Expand-Archive -LiteralPath wasm-component-ld-v0.5.3-x86_64-windows.zip -DestinationPath .
cp wasm-component-ld-v0.5.3-x86_64-windows/wasm-component-ld.exe wasi-sdk/bin
