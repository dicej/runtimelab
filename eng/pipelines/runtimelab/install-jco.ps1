$RootPath = $Args[0]

$NpmExePath = $Env:NPM_EXECUTABLE

Set-Location -Path $RootPath

$NpmCommand = "$NpmExePath install @bytecodealliance/jco"
Invoke-Expression $NpmCommand

$NpmCommand = "$NpmExePath install @bytecodealliance/preview2-shim"
Invoke-Expression $NpmCommand

