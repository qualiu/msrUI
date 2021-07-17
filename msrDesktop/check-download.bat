@echo off
if "%PATH:~-1%" == "\" set "PATH=%PATH:~0,-1%"
SetLocal EnableExtensions EnableDelayedExpansion

if "%~1" == "" (
    echo Usage:   %0  SaveFolder
    echo Example: %0  %~dp0
    exit /b -1
)

set SaveFolder=%~dp1%~nx1
if "%SaveFolder:~-1%" == "\" set "SaveFolder=%SaveFolder:~0,-1%"
if not exist !SaveFolder! md !SaveFolder!

echo "!PATH!" | findstr /I /L /C:"!SaveFolder!;" >nul || set "PATH=!SaveFolder!;!PATH!"

call :Download_File msr.exe || ( EndLocal &  exit /b -1 )
call :Download_File nin.exe || ( EndLocal &  exit /b -1 )
@REM call :Download_File psall.bat || ( EndLocal &  exit /b -1 )
@REM call :Download_File pskill.bat || ( EndLocal &  exit /b -1 )

EndLocal & set "PATH=%PATH%" & exit /b 0

:Download_File
    set name=%1
    where %name% 2>nul >nul && exit /b 0
    where wget.exe 2>nul >nul
    if !ERRORLEVEL! EQU 0 (
        wget https://github.com/qualiu/msr/raw/master/tools/%name% -O %name%.tmp --no-check-certificate -q ^
        && move /y %name%.tmp %name% && icacls %name% /grant %USERNAME%:RX && move /y %name% !SaveFolder!\
    ) else (
        PowerShell -Command "$ProgressPreference = 'SilentlyContinue'; [Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12; Invoke-WebRequest -Uri 'https://github.com/qualiu/msr/raw/master/tools/%name%' -OutFile %name%.tmp" ^
        && move /y %name%.tmp %name% && icacls %name% /grant %USERNAME%:RX && move /y %name% !SaveFolder!\
    )

    where %name% >nul
