@echo off
SetLocal EnableExtensions EnableDelayedExpansion
pushd %~dp0

call %~dp0\check-download.bat %~dp0 || exit /b -1

call :Basic_Install || exit /b -1

echo. & echo Check and install .NET Core SDK ...
set findDotNetCmd=msr -rp . -f "\.csproj$" -t "^\s*<TargetFramework>\s*\w+?\D+(\d+[\.\d]*).*"
!findDotNetCmd! -M
if !ERRORLEVEL! GTR 0 (
    for /f "tokens=*" %%a in ('!findDotNetCmd! -o "\1" -PAC ^| nin nul -uPAC') do call :Check_Install_DotNetCore %%a || exit /b -1
)

exit /b 0

:Basic_Install
    where choco.exe /q || (
        echo Install choco by Admin with command from: https://chocolatey.org/install
        powershell "Set-ExecutionPolicy Bypass -Scope Process -Force; [System.Net.ServicePointManager]::SecurityProtocol = [System.Net.ServicePointManager]::SecurityProtocol -bor 3072; iex ((New-Object System.Net.WebClient).DownloadString('https://chocolatey.org/install.ps1'))"
        call %ALLUSERSPROFILE%\chocolatey\bin\RefreshEnv.cmd
        where choco.exe /q || ( echo Failed to install choco. 1>&2 & exit /b -1 )
    )

    echo Check and install basic tools by Admin:
    where wget.exe /q || choco install -y wget || exit /b -1
    where curl.exe /q || choco install -y curl || exit /b -1
    call %ALLUSERSPROFILE%\chocolatey\bin\RefreshEnv.cmd
    exit /b 0


:Check_Install_DotNetCore
    set version=%~1
    echo Check and install .NET SDK !version! ... | msr -aPA -ie "(.NET SDK \S+)"
    where dotnet.exe /q 2>nul && (
        dotnet --list-sdks | msr -ix !version! -PM
        if !ERRORLEVEL! EQU 1 (
            for /f "tokens=*" %%a in ('dotnet --list-sdks ^| msr -x !version! -t ".*?\[(.+?)\].*" -o "\1" -PAC') do (
                if exist "%%a\!version!\Sdks\Microsoft.NET.Sdk\Sdk" (
                    echo Found %%a\!version!\Sdks\Microsoft.NET.Sdk\Sdk | msr -aPA -ix !version! -e .+
                    exit /b 0
                ) else (
                    echo Not found %%a\!version!\Sdks\Microsoft.NET.Sdk\Sdk | msr -aPA -it "Not.*?(!version!).*"
                )
            )
        )
    )

    choco list -ali | msr -it "dotnetcore\D*!version!" -M || (
        echo Already installed dotnetcore !version! by choco as above.
        exit /b
    )

    echo choco install -y dotnetcore-sdk --version=!version! | msr -XM || (
        call :Install_DotNetCore_Raw !version! || exit /b -1
    )
    exit /b 0

:Install_DotNetCore_Raw
    set version=%~1
    msr -z "!version!" -t "^(\d+\.\d+).*" -o "Follow doc: https://dotnet.microsoft.com/download/dotnet/\1 to install dotnet !version!" -PAC
    curl https://dotnet.microsoft.com/download/dotnet/!version! --silent --show-error ^
        | msr -it ".*?/download/([\w/\.-]*?\b!version!\S*windows\S*?x64\S*?installer[\w\.-]*).*" -o "curl https://dotnet.microsoft.com/download/\1 --silent --show-error" -H 1 -X -PAC ^
        | msr -it ".*?(http.*?)/([\w\.-]*?!version!.*?\.(exe|msi)).*" -o "wget -q \1/\2 -O \2.tmp && move /y \2.tmp \2 && call \2 /install /passive && del /f \2" -H 1 -X -M ^
        || exit /b -1
    exit /b 0
