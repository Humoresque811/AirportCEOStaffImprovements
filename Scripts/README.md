# AirportCEO Tweaks - Build Scripts

This directory contains cross-platform build automation scripts for the AirportCEO Tweaks project.

## Prerequisites

### PowerShell Core

All scripts use **PowerShell Core (pwsh)** for cross-platform compatibility:

- **Windows**: [Download PowerShell Core](https://github.com/PowerShell/PowerShell#get-powershell)
- **macOS**: `brew install --cask powershell`
- **Linux**: [Installation instructions](https://docs.microsoft.com/en-us/powershell/scripting/install/installing-powershell-core-on-linux)

### .NET SDK

Ensure you have .NET SDK installed (project uses .NET Framework 4.6):

```bash
dotnet --version
```

## .NET Local Tools

This project uses a local tools manifest (`.config/dotnet-tools.json`) to manage development tools.

### Restore Tools

Before first use, restore the local tools:

```bash
dotnet tool restore
```

### Available Tools

- **dotnet-format** - Code formatting tool
  ```bash
  dotnet format
  ```
- **dotnet-script** - C# scripting support
  ```bash
  dotnet script
  ```
- **dotnet-outdated** - Check for outdated NuGet packages
  ```bash
  dotnet outdated
  ```

## Build Scripts

### launch-game.ps1

Standalone script to launch Airport CEO through Steam protocol. Works on Windows, macOS, and Linux.

**Usage:**

```powershell
# Launch game through Steam
./scripts/launch-game.ps1
```

**Requirements:**

- Steam must be installed and running
- Airport CEO must be in your Steam library
- Steam must not be in offline mode

**How it works:**
Uses Steam's protocol handler (`steam://rungameid/673610`) to launch the game properly with all Steam features (DRM, cloud saves, achievements, etc.). This ensures the game initializes correctly, unlike direct executable launching.

This script is automatically called by `build.ps1` and `build-release.ps1` when `-Launch` is enabled.

### build.ps1

Build the solution or specific projects in **Debug** configuration. **Automatically launches Airport CEO after successful build** unless `-Launch:$false` is specified.

**Usage:**

```powershell
# Build entire solution (launches game by default)
./scripts/build.ps1

# Build specific project (launches game by default)
./scripts/build.ps1 -Project AirportCEOAircraft

# Build without launching game
./scripts/build.ps1 -Launch:$false

# Build specific project without launching game
./scripts/build.ps1 -Project AirportCEOAircraft -Launch:$false
```

**Available Projects:**

- `AirportCEOTweaksCore`
- `AirportCEOAircraft`
- `AirportCEOFlightLimitTweak`
- `AirportCEORunways`
- `AirportCEOPlanningReplanned`

### build-release.ps1

Build the solution or specific projects in **Release** configuration. **Automatically launches Airport CEO after successful build** unless `-Launch:$false` is specified.

**Usage:**

```powershell
# Build entire solution (launches game by default)
./scripts/build-release.ps1

# Build specific project (launches game by default)
./scripts/build-release.ps1 -Project AirportCEOTweaksCore

# Build without launching game
./scripts/build-release.ps1 -Launch:$false

# Build specific project without launching game
./scripts/build-release.ps1 -Project AirportCEOAircraft -Launch:$false
```

## Platform Notes

### Windows

Scripts can be run directly from PowerShell or PowerShell Core:

```powershell
.\scripts\build.ps1
```

### macOS/Linux

Scripts include a shebang (`#!/usr/bin/env pwsh`) and can be executed directly if made executable:

```bash
chmod +x scripts/*.ps1
./scripts/build.ps1
```

Or run explicitly with pwsh:

```bash
pwsh scripts/build.ps1
```

## Manual Build Commands

If you prefer using `dotnet` directly:

```bash
# Build entire solution (Debug)
dotnet build AirportCEOTweaks.sln --configuration Debug

# Build entire solution (Release)
dotnet build AirportCEOTweaks.sln --configuration Release

# Build specific project
dotnet build AirportCEOAircraft/AirportCEOAircraft.csproj --configuration Debug
```

## Troubleshooting

### Permission Denied (macOS/Linux)

Make scripts executable:

```bash
chmod +x scripts/*.ps1
```

### PowerShell Execution Policy (Windows)

If you get an execution policy error:

```powershell
Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope CurrentUser
```

### .NET SDK Not Found

Ensure .NET SDK is installed and in your PATH:

```bash
dotnet --version
```
