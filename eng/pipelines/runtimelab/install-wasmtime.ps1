Invoke-WebRequest -Uri https://github.com/bytecodealliance/wasmtime/releases/download/v21.0.1/wasmtime-v21.0.1-x86_64-windows.zip -OutFile wasmtime-v21.0.1-x86_64-windows.zip

mkdir wasmtime\bin

Expand-Archive -LiteralPath wasmtime-v21.0.1-x86_64-windows.zip -DestinationPath .
del wasmtime-v21.0.1-x86_64-windows.zip
move wasmtime-v21.0.1-x86_64-windows\wasmtime.exe wasmtime\bin\
Remove-Item -Recurse wasmtime-v21.0.1-x86_64-windows
