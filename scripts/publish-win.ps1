param(
    [string]$Configuration = "Release",
    [string]$Version = "1.0.0",
    [string]$AssemblyVersion = "1.0.0.0",
    [string]$FileVersion = "1.0.0.0",
    [string]$InformationalVersion = "1.0.0+local"
)

$ErrorActionPreference = "Stop"

$repoRoot = Resolve-Path (Join-Path $PSScriptRoot "..")
$project = Join-Path $repoRoot "src\LaserMarkingApp\LaserMarkingApp.vbproj"
$dist = Join-Path $repoRoot "dist"

if (Test-Path $dist) {
    Remove-Item $dist -Recurse -Force
}

New-Item -ItemType Directory -Path $dist | Out-Null

$runtimes = @("win-x64", "win-x86")

foreach ($runtime in $runtimes) {
    $outDir = Join-Path $dist "publish-$runtime"
    dotnet publish $project `
        -c $Configuration `
        -r $runtime `
        --self-contained true `
        -p:PublishSingleFile=true `
        -p:PublishTrimmed=false `
        -p:IncludeNativeLibrariesForSelfExtract=true `
        -p:DebugType=None `
        -p:DebugSymbols=false `
        -p:Version=$Version `
        -p:AssemblyVersion=$AssemblyVersion `
        -p:FileVersion=$FileVersion `
        -p:InformationalVersion=$InformationalVersion `
        -o $outDir

    $sourceExe = Join-Path $outDir "LaserMarkingApp.exe"
    $targetExe = Join-Path $dist "LaserMarkingApp-$runtime.exe"
    Copy-Item $sourceExe $targetExe -Force
    Remove-Item $outDir -Recurse -Force
}

Write-Host "Published release executables in $dist"
Write-Host "App version: $InformationalVersion"
