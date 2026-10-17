param([switch]$SkipRestore)
$ErrorActionPreference = 'Stop'
Push-Location $PSScriptRoot
try {
    $argsList = @('build', 'Source/AnimalTab.csproj', '--configuration', 'Release')
    if ($SkipRestore) { $argsList += '--no-restore' }
    & dotnet @argsList
    if ($LASTEXITCODE -ne 0) { throw 'Animal Tab build failed.' }
    & dotnet run --project Tests/AnimalTab.Tests.csproj --configuration Release -- (Join-Path $PSScriptRoot 'Assemblies/AnimalTab.dll')
    if ($LASTEXITCODE -ne 0) { throw 'Regression tests failed.' }
    Write-Output 'Build and regression tests passed.'
} finally { Pop-Location }
