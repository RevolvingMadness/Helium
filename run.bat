@echo off

clear
dotnet run -- main.he -r "C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.7.2\mscorlib.dll"
main.exe

echo Return Value:
echo %ERRORLEVEL%

echo IL:
ilspycmd main.exe