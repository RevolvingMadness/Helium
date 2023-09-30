Clear-Host
dotnet run main.he
llc -filetype=obj main.bc
gcc main.obj -o build/main.exe -nostdlib
./build/main