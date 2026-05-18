$ErrorActionPreference = 'Stop'

$RepoRoot = (Resolve-Path (Join-Path $PSScriptRoot '..')).Path
$ManifestPath = Join-Path $RepoRoot 'eng/roslyn-demagic-package-expected-diagnostics.json'
$SampleProject = Join-Path $RepoRoot 'examples/Roslyn.DeMagic.PackageSmoke/Roslyn.DeMagic.PackageSmoke.csproj'
$NuGetConfig = Join-Path $RepoRoot 'examples/Roslyn.DeMagic.PackageSmoke/NuGet.config'
$ArtifactsDir = Join-Path $RepoRoot 'artifacts/packages'
$BuildLog = Join-Path $RepoRoot 'artifacts/roslyn-demagic-package-smoke-build.log'
$GlobalPackageDir = Join-Path $HOME '.nuget/packages/roslyn.demagic/0.1.0'

New-Item -ItemType Directory -Force -Path $ArtifactsDir | Out-Null
New-Item -ItemType Directory -Force -Path (Split-Path $BuildLog -Parent) | Out-Null
if (Test-Path $GlobalPackageDir) {
  Remove-Item -Recurse -Force $GlobalPackageDir
}

dotnet pack (Join-Path $RepoRoot 'src/Roslyn.DeMagic/Roslyn.DeMagic.csproj') `
  --configuration Release `
  -o $ArtifactsDir | Out-Null

dotnet restore $SampleProject --configfile $NuGetConfig --no-cache | Out-Null
dotnet build $SampleProject --no-restore --nologo --verbosity minimal *>&1 | Tee-Object -FilePath $BuildLog | Out-Null

$manifest = Get-Content $ManifestPath | ConvertFrom-Json
$lines = Get-Content $BuildLog
$regex = '^(?<file>.+\.cs)\((?<line>\d+),(?<col>\d+)\): (?<severity>warning|error|info) (?<id>DM00[12]):'

function Normalize-File([string]$path) {
  $normalized = $path.Replace('\', '/')
  $marker = '/Samples/'
  $index = $normalized.LastIndexOf($marker)
  if ($index -ge 0) {
    return 'Samples/' + $normalized.Substring($index + $marker.Length)
  }
  return [System.IO.Path]::GetFileName($normalized)
}

$observed = @()
$seen = @{}
foreach ($line in $lines) {
  if ($line -match $regex) {
    $file = Normalize-File $matches['file']
    $severity = $matches['severity'].ToLowerInvariant()
    $key = "$file|$($matches['id'])|$severity"
    if ($seen.ContainsKey($key)) {
      continue
    }

    $seen[$key] = $true
    $observed += [pscustomobject]@{
      File = $file
      Id = $matches['id']
      Severity = $severity
      Raw = $line.Trim()
    }
  }
}

$expectedKeys = @{}
foreach ($row in $manifest.expectedDiagnostics) {
  $key = "$($row.file)|$($row.id)|$($row.severity.ToLowerInvariant())"
  if (-not $expectedKeys.ContainsKey($key)) { $expectedKeys[$key] = 0 }
  $expectedKeys[$key] += 1
}

$observedKeys = @{}
foreach ($row in $observed) {
  $key = "$($row.File)|$($row.Id)|$($row.Severity)"
  if (-not $observedKeys.ContainsKey($key)) { $observedKeys[$key] = 0 }
  $observedKeys[$key] += 1
}

$missing = @()
foreach ($entry in $expectedKeys.GetEnumerator()) {
  $observedCount = if ($observedKeys.ContainsKey($entry.Key)) { $observedKeys[$entry.Key] } else { 0 }
  for ($i = $observedCount; $i -lt $entry.Value; $i++) {
    $missing += $entry.Key
  }
}

$unexpected = @()
foreach ($entry in $observedKeys.GetEnumerator()) {
  $expectedCount = if ($expectedKeys.ContainsKey($entry.Key)) { $expectedKeys[$entry.Key] } else { 0 }
  for ($i = $expectedCount; $i -lt $entry.Value; $i++) {
    $unexpected += $entry.Key
  }
}

$cleanFiles = @($manifest.expectedCleanFiles)
$cleanHits = @($observed | Where-Object { $cleanFiles -contains $_.File } | ForEach-Object { $_.Raw })

if ($missing.Count -gt 0 -or $unexpected.Count -gt 0 -or $cleanHits.Count -gt 0) {
  if ($missing.Count -gt 0) {
    Write-Host 'Missing expected diagnostics:'
    $missing | ForEach-Object { Write-Host "  - $_" }
  }
  if ($unexpected.Count -gt 0) {
    Write-Host 'Unexpected diagnostics:'
    $unexpected | ForEach-Object { Write-Host "  - $_" }
  }
  if ($cleanHits.Count -gt 0) {
    Write-Host 'Diagnostics reported for expected-clean files:'
    $cleanHits | ForEach-Object { Write-Host "  - $_" }
  }
  exit 1
}

Write-Host 'Roslyn.DeMagic package validation passed.'
Write-Host "Validated $($manifest.expectedDiagnostics.Count) expected diagnostics and $($manifest.expectedCleanFiles.Count) clean files."
