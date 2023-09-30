Clear-Host
dotnet run main.he
llc -filetype=obj build/main.bc
gcc build/main.obj -o build/main.exe -nostdlib
./build/main.exe