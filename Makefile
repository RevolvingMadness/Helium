all: build

build:
	dotnet build

run: build
	clear
	./bin/Debug/net7.0/Helium.exe main.he