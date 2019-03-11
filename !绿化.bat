@ECHO OFF & CD /D %~DP0 & TITLE  绿化
>NUL 2>&1 REG.exe query "HKU\S-1-5-19" || (
    ECHO SET UAC = CreateObject^("Shell.Application"^) > "%TEMP%\Getadmin.vbs"
    ECHO UAC.ShellExecute "%~f0", "%1", "", "runas", 1 >> "%TEMP%\Getadmin.vbs"
    "%TEMP%\Getadmin.vbs"
    DEL /f /q "%TEMP%\Getadmin.vbs" 2>NUL
    Exit /b
)

taskkill /f /im Youku*  >NUL 2>NUL
taskkill /f /im ikuacc* >NUL 2>NUL

rd/s/q "%AppData%\Teiron" 2>NUL
rd/s/q "%AppData%\youku\ppzs" 2>NUL
rd/s/q "%AppData%\youku\upgrade" 2>NUL

xcopy /e/i/y "$APPDATA\youku" "%AppData%\youku" >NUL 2>NUL
xcopy /e/i/y "$APPDATA\ytmediacenter" "%AppData%\ytmediacenter" >NUL 2>NUL

:: regsvr32 /s "cmc_plugins\ykcool.dll"
:: regsvr32 /s "cmc_plugins\coreplay.dll"
:: regsvr32 /s "cmc_plugins\npYoukuAgent.dll"
:: If "%PROCESSOR_ARCHITECTURE%"=="AMD64" regsvr32 /s "cmc_plugins\X64\ykcool64.dll"
:: If "%PROCESSOR_ARCHITECTURE%"=="AMD64" regsvr32 /s "cmc_plugins\X64\coreplay64.dll"
:: If "%PROCESSOR_ARCHITECTURE%"=="AMD64" regsvr32 /s "cmc_plugins\X64\npYoukuAgent_x64.dll"
copy /y "D3DX9_43.dll" "%WinDir%\System32/" >NUL 2>NUL
copy /y "d3dcompiler_43.dll" "%WinDir%\System32/" >NUL 2>NUL
If "%PROCESSOR_ARCHITECTURE%"=="AMD64" copy /y "D3DX9_43.dll" "%WinDir%\SysWOW64/" >NUL 2>NUL
If "%PROCESSOR_ARCHITECTURE%"=="AMD64" copy /y "d3dcompiler_43.dll" "%WinDir%\SysWOW64/" >NUL 2>NUL
reg add "HKCU\Software\YouKu" /v "first_show_desktop" /f /t REG_SZ /d "1" >NUL
reg add "HKCU\Software\YouKu\YoukuClient" /v "Path" /f /t REG_SZ /d "%~dp0YoukuClient\YoukuDesktop.exe" >NUL
reg add "HKCU\Software\YouKu\iKuAcc" /v "CmcPath" /f /t REG_SZ /d "%~dp0YoukuClient\YoukuMediaCenter.exe" >NUL
:: reg add "HKCU\Software\MozillaPlugins\youku.com/YoukuAgent" /f /v "Path" /d "%~dp0cmc_plugins\npYoukuAgent.dll" >NUL
:: reg add "HKCU\Software\MozillaPlugins\youku.com/YoukuAgent_x86_64" /f  /v "Path" /d "%~dp0cmc_plugins\X64\npYoukuAgent_x64.dll" >NUL
::这条很关键非常重要，没有这条键值任务栏通知区域优酷右键退出无反应！
if "%PROCESSOR_ARCHITECTURE%"=="x86" reg add "HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\YoukuClient" /f /v "Install_Dir" /t  REG_SZ  /d "%~dp0YoukuClient\" >NUL
If "%PROCESSOR_ARCHITECTURE%"=="AMD64" reg add "HKLM\SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall\YoukuClient" /f  /v "Install_Dir" /t  REG_SZ  /d "%~dp0YoukuClient\" >NUL
If Exist "%Public%"  reg add "HKCU\Software\Microsoft\Windows NT\CurrentVersion\AppCompatFlags\Layers" /f /v "%~dp0YoukuClient\YoukuMediaCenter.exe" /d "~ RUNASADMIN" >NUL

CLS & ECHO.&ECHO 绿化完成，是否创建桌面快捷方式？
ECHO.&ECHO 是按任意键，否直接关闭窗口即可！&&PAUSE >NUL

mshta VBScript:Execute("Set a=CreateObject(""WScript.Shell""):Set b=a.CreateShortcut(a.SpecialFolders(""Desktop"") & ""\优酷视频.lnk""):b.TargetPath=""%~dp0YoukuClient\Youku.exe"":b.WorkingDirectory=""%~dp0"":b.Save:close")

CLS && ECHO.&ECHO 创建完成！ &&PAUSE>NUL & EXIT