param(
    [string]$Configuration = "Release"
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
        -o $outDir

    $sourceExe = Join-Path $outDir "LaserMarkingApp.exe"
    $targetExe = Join-Path $dist "LaserMarkingApp-$runtime.exe"
    Copy-Item $sourceExe $targetExe -Force
    Remove-Item $outDir -Recurse -Force
}

Write-Host "Published release executables in $dist"
