SETLOCAL
color 7
call "%VS120COMNTOOLS%VsDevCmd.bat"

set FLAG1=/p:Configuration=Debug;Platform=x86
set FLAG2=/p:Configuration=Release;Platform=x64

msbuild /t:Clean %FLAG1%
msbuild /t:Build %FLAG1%
IF %ERRORLEVEL% NEQ 0 SET ERROR=1

msbuild /t:Clean %FLAG2%
msbuild /t:Build %FLAG2%
IF %ERRORLEVEL% NEQ 0 SET ERROR=1

MSTest /testcontainer:TestLanguageTools\bin\Debug\TestLanguageTools.dll
IF %ERRORLEVEL% NEQ 0 SET ERROR=1

MSTest /testcontainer:TestLanguageTools\bin\Release\TestLanguageTools.dll
IF %ERRORLEVEL% NEQ 0 SET ERROR=1

@if DEFINED ERROR (
	call ColorText C "There were errors..."
) else ( call ColorText A "Success!" )
echo.
pause