param([string]$OutputDirectory = (Join-Path $PSScriptRoot 'dist'))
$ErrorActionPreference = 'Stop'
$dll = Join-Path $PSScriptRoot 'Assemblies/AnimalTab.dll'
if (!(Test-Path -LiteralPath $dll)) { throw 'Build the mod with build.ps1 first.' }
# Use a new staging directory every run; never recursively delete an existing tree.
$stamp = Get-Date -Format 'yyyyMMdd-HHmmss-fff'
$stage = Join-Path $OutputDirectory ("staging-$stamp/AnimalTab-Unofficial")
New-Item -ItemType Directory -Path (Join-Path $stage 'About'), (Join-Path $stage 'Assemblies') -Force | Out-Null
foreach ($folder in @('Defs', 'Languages', 'Textures')) {
    Copy-Item -LiteralPath (Join-Path $PSScriptRoot $folder) -Destination $stage -Recurse
}
foreach ($file in @('About.xml', 'Preview.png')) {
    Copy-Item -LiteralPath (Join-Path $PSScriptRoot "About/$file") -Destination (Join-Path $stage 'About')
}
Copy-Item -LiteralPath $dll -Destination (Join-Path $stage 'Assemblies')
foreach ($file in @('LICENSE', 'CREDITS.md', 'CHANGELOG.md', 'Readme.md')) {
    Copy-Item -LiteralPath (Join-Path $PSScriptRoot $file) -Destination $stage
}
[xml]$metadata = Get-Content -LiteralPath (Join-Path $stage 'About/About.xml') -Raw
if ($metadata.ModMetaData.packageId -cne 'Fluffy.AnimalTab') { throw 'Unexpected release package ID.' }
if (Test-Path -LiteralPath (Join-Path $stage 'About/PublishedFileId.txt')) { throw 'An upstream Workshop ID must not be packaged.' }
$dlls = @(Get-ChildItem -LiteralPath (Join-Path $stage 'Assemblies') -Filter '*.dll')
if ($dlls.Count -ne 1 -or $dlls[0].Name -ne 'AnimalTab.dll') { throw 'Unexpected runtime DLLs in package.' }
Get-ChildItem -LiteralPath $stage -Recurse -File | ForEach-Object {
    $relative = $_.FullName.Substring($stage.Length + 1).Replace('\', '/')
    '{0}  {1}' -f (Get-FileHash -LiteralPath $_.FullName -Algorithm SHA256).Hash.ToLowerInvariant(), $relative
} | Set-Content -LiteralPath (Join-Path $stage 'SHA256SUMS.txt')
$releaseVersion = $metadata.ModMetaData.modVersion
if ($releaseVersion -notmatch '^\d+\.\d+\.\d+$') { throw 'Invalid release version in About.xml.' }
$zip = Join-Path $OutputDirectory "AnimalTab-Unofficial-$releaseVersion-$stamp.zip"
Compress-Archive -LiteralPath $stage -DestinationPath $zip
Write-Output "Workshop folder: $stage"
Write-Output "Release archive: $zip"
