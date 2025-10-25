#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Build the AirportCEO Tweaks solution in Debug configuration without launching the game.

.DESCRIPTION
    Wrapper script that calls the main build script with Launch parameter set to false.
#>

param(
    [string]$Project = ""
)

$ErrorActionPreference = "Stop"

# Get the repository root (parent of scripts directory)
$BuildScript = Join-Path $PSScriptRoot "build.ps1"

# Call the main build script with Launch set to false
if ([string]::IsNullOrEmpty($Project)) {
    & $BuildScript -Launch $false
} else {
    & $BuildScript -Project $Project -Launch $false
}
