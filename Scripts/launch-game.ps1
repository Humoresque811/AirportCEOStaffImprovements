#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Launch Airport CEO through Steam.

.DESCRIPTION
    Cross-platform script to launch Airport CEO using Steam protocol.
    This ensures the game initializes correctly with Steam's DRM and features.
    Works on Windows, macOS, and Linux.

.EXAMPLE
    ./launch-game.ps1
    Launches Airport CEO through Steam.

.NOTES
    Requires Steam to be installed and running.
    Uses Steam App ID: 673610 (Airport CEO)
#>

param()

$ErrorActionPreference = "Stop"

# Airport CEO Steam App ID
$SteamAppId = "673610"
$SteamUrl = "steam://rungameid/$SteamAppId"

Write-Host "==================================================" -ForegroundColor Cyan
Write-Host "Launching Airport CEO through Steam" -ForegroundColor Cyan
Write-Host "==================================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "Steam App ID: $SteamAppId" -ForegroundColor Gray
Write-Host "Using protocol: $SteamUrl" -ForegroundColor Gray
Write-Host ""

try {
    if ($IsWindows -or $PSVersionTable.Platform -eq "Win32NT" -or !$PSVersionTable.Platform) {
        # Windows: Use Start-Process with steam:// protocol
        Write-Host "Launching on Windows..." -ForegroundColor Yellow
        Start-Process $SteamUrl
    }
    elseif ($IsMacOS) {
        # macOS: Use 'open' command
        Write-Host "Launching on macOS..." -ForegroundColor Yellow
        & open $SteamUrl
    }
    elseif ($IsLinux) {
        # Linux: Try xdg-open first, fall back to steam command
        Write-Host "Launching on Linux..." -ForegroundColor Yellow
        
        # Check if xdg-open is available
        if (Get-Command xdg-open -ErrorAction SilentlyContinue) {
            & xdg-open $SteamUrl
        }
        # Fall back to steam command
        elseif (Get-Command steam -ErrorAction SilentlyContinue) {
            & steam "steam://rungameid/$SteamAppId"
        }
        else {
            Write-Warning "Neither 'xdg-open' nor 'steam' command found."
            Write-Host ""
            Write-Host "Please install xdg-utils or ensure Steam is in your PATH:" -ForegroundColor Yellow
            Write-Host "  sudo apt-get install xdg-utils" -ForegroundColor Gray
            exit 1
        }
    }
    
    Write-Host ""
    Write-Host "==================================================" -ForegroundColor Green
    Write-Host "Launch command sent to Steam successfully!" -ForegroundColor Green
    Write-Host "==================================================" -ForegroundColor Green
    Write-Host ""
    Write-Host "If the game doesn't start:" -ForegroundColor Yellow
    Write-Host "  1. Make sure Steam is running" -ForegroundColor Gray
    Write-Host "  2. Verify Airport CEO is installed in your Steam library" -ForegroundColor Gray
    Write-Host "  3. Check Steam is not in offline mode" -ForegroundColor Gray
    
    exit 0
}
catch {
    Write-Host ""
    Write-Host "==================================================" -ForegroundColor Red
    Write-Host "Failed to launch game through Steam" -ForegroundColor Red
    Write-Host "==================================================" -ForegroundColor Red
    Write-Host ""
    Write-Host "Error: $($_.Exception.Message)" -ForegroundColor Red
    Write-Host ""
    Write-Host "Please ensure:" -ForegroundColor Yellow
    Write-Host "  - Steam is installed and running" -ForegroundColor Gray
    Write-Host "  - Airport CEO is in your Steam library" -ForegroundColor Gray
    Write-Host "  - You can manually launch the game from Steam" -ForegroundColor Gray
    
    exit 1
}
