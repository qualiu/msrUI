@echo off
SetLocal EnableExtensions EnableDelayedExpansion
pushd %~dp0

call %~dp0\check-download.bat %~dp0
if %ERRORLEVEL% EQU 0 (
    msr -rp . -l -f "\.sln" -PAC | msr -t "(.+)" -o "dotnet publish \1 -c Release /v:m" -X -V -ne0 -M || exit /b -1
    msr -rp . -l -f exe -d "^bin$" --nd "^obj$" --wt --sz -PM -W
) else (
    for /f "tokens=*" %%a in ('dir /s /b *.sln') do (
        echo dotnet publish %%a -c Release /v:m
        dotnet publish %%a -c Release /v:m || exit /b -1
    )
)

exit /b 0
