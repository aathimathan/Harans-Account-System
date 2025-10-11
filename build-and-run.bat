@echo off
cd /d "%~dp0"
echo Building Haran Invoice Software...
dotnet build
if %ERRORLEVEL% == 0 (
    echo Build successful. Starting application...
    dotnet run
) else (
    echo Build failed. Please check the error messages above.
    pause
)