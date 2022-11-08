# Paint

## Important Links
- [Github](https://github.com/ExoKomodo/paint)
- [Engine](https://github.com/ExoKomodo/womb)
- [Jenkins - CI](https://jenkins.exokomodo.com/job/paint)
- [Jenkins - Base Image](https://jenkins.exokomodo.com/job/paint-base)

## Setup

### Submodules
When cloning this repo, clone like so
```bash
git clone --recurse-submodules git@github.com:ExoKomodo/paint.git
```
If you forget `--recurse-submodules` in the clone, you can fix this by running these commands in the git repo
```bash
git submodule init
git submodule update --recursive --remote
```

### Install Dotnet

#### Linux
On Ubuntu Jammy and up, install dotnet with `apt`
```bash
sudo apt install dotnet6
```

#### Mac
Install .NET 6 via the [downloaded installer](https://dotnet.microsoft.com/en-us/download)

### Install SDL2

#### Linux
Install sdl2 dev libraries
```bash
sudo apt install libsdl2-dev
```
#### Mac
Install [Homebrew](https://brew.sh)
Use `brew` to install sdl2 dev libraries
```bash
brew install sdl2
```

## Build
Using the `dotnet` CLI is the easiest way to build and manage this project.
To build the project, you have these options
### Build all
#### Simple
```bash
dotnet build
```
#### Configure Options
##### Debug
```bash
dotnet restore
dotnet build --no-restore --configuration Debug
```
##### Release
```bash
dotnet restore
dotnet build --no-restore --configuration Release
```

### Build the game
#### Simple
```bash
dotnet build src/Paint
```
##### Debug
```bash
dotnet restore
dotnet build --no-restore --configuration Debug src/Paint
```
##### Release
```bash
dotnet restore
dotnet build --no-restore --configuration Release src/Paint
```

## Play the Game
### Simple
```bash
dotnet run --project src/Paint
```
### Configure Options
#### Debug
```bash
dotnet restore
dotnet run --no-restore --configuration Debug --project src/Paint
```
#### Release
```bash
dotnet restore
dotnet run --no-restore --configuration Release --project src/Paint
```

## Create a Binary
### Simple
```bash
dotnet publish src/Paint
```
### Configure Options
#### Debug
```bash
dotnet restore
dotnet publish --no-restore --configuration Debug src/Paint
```
#### Release
```bash
dotnet restore
dotnet publish --no-restore --configuration Debug src/Paint
```