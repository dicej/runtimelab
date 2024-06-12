mkdir "%1" 2>nul
cd /D "%1"

echo Installing Wasmtime

powershell -NoProfile -NoLogo -ExecutionPolicy ByPass -File "%~dp0install-wasmtime.ps1"
if %errorlevel% NEQ 0 goto fail

echo Setting WASMTIME_EXECUTABLE to %1\wasmtime\bin\wasmtime.exe
echo ##vso[task.setvariable variable=WASMTIME_EXECUTABLE]%1\wasmtime\bin\wasmtime.exe

exit /b 0

fail:
echo "Failed to install wasmtime"
exit /b 1
