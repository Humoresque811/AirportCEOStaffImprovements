#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Build the AirportCEO Tweaks solution or specific projects in Release configuration.

.DESCRIPTION
    Cross-platform build script that compiles the solution or individual projects.
    Works on Windows, macOS, and Linux with PowerShell Core.

.PARAMETER Project
    Optional. Name of a specific project to build (e.g., "AirportCEOAircraft").
    If not specified, builds the entire solution.

.PARAMETER Launch
    Optional. Launch Airport CEO after successful build.

.EXAMPLE
    ./build-release.ps1
    Builds the entire solution in Release configuration.

.EXAMPLE
    ./build-release.ps1 -Project AirportCEOAircraft
    Builds only the AirportCEOAircraft project in Release configuration.

.EXAMPLE
    ./build-release.ps1 -Launch
    Builds the entire solution and launches Airport CEO.
#>

param(
    [string]$Project = "",
    [bool]$Launch = $true
)

$ErrorActionPreference = "Stop"

# Get the repository root (parent of scripts directory)
$RepoRoot = Split-Path $PSScriptRoot -Parent
$SolutionFile = Join-Path $RepoRoot "AirportCEOTweaks.sln"

Write-Host "==================================================" -ForegroundColor Cyan
Write-Host "Building AirportCEO Tweaks (Release)" -ForegroundColor Cyan
Write-Host "==================================================" -ForegroundColor Cyan
Write-Host ""

# Check if solution file exists
if (-not (Test-Path $SolutionFile)) {
    Write-Error "Solution file not found: $SolutionFile"
    exit 1
}

try {
    if ([string]::IsNullOrEmpty($Project)) {
        Write-Host "Building entire solution..." -ForegroundColor Yellow
        Write-Host "Command: dotnet build `"$SolutionFile`" --configuration Release" -ForegroundColor Gray
        Write-Host ""
        
        dotnet build "$SolutionFile" --configuration Release
    }
    else {
        $ProjectFile = Join-Path $RepoRoot "$Project\$Project.csproj"
        
        if (-not (Test-Path $ProjectFile)) {
            Write-Error "Project file not found: $ProjectFile"
            exit 1
        }
        
        Write-Host "Building project: $Project" -ForegroundColor Yellow
        Write-Host "Command: dotnet build `"$ProjectFile`" --configuration Release" -ForegroundColor Gray
        Write-Host ""
        
        dotnet build "$ProjectFile" --configuration Release
    }
    
    if ($LASTEXITCODE -eq 0) {
        Write-Host ""
        Write-Host "==================================================" -ForegroundColor Green
        Write-Host "Build completed successfully!" -ForegroundColor Green
        Write-Host "==================================================" -ForegroundColor Green
        
        # Launch game if requested
        if ($Launch) {
            Write-Host ""
            $LaunchScript = Join-Path $PSScriptRoot "launch-game.ps1"
            & $LaunchScript
        }
    }
    else {
        Write-Host ""
        Write-Host "==================================================" -ForegroundColor Red
        Write-Host "Build failed with exit code: $LASTEXITCODE" -ForegroundColor Red
        Write-Host "==================================================" -ForegroundColor Red
        exit $LASTEXITCODE
    }
}
catch {
    Write-Host ""
    Write-Host "==================================================" -ForegroundColor Red
    Write-Host "Build failed with error:" -ForegroundColor Red
    Write-Host $_.Exception.Message -ForegroundColor Red
    Write-Host "==================================================" -ForegroundColor Red
    exit 1
}


