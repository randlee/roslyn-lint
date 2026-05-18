#!/usr/bin/env bash
set -euo pipefail

repo_root="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
manifest_path="$repo_root/eng/roslyn-demagic-package-expected-diagnostics.json"
sample_project="$repo_root/examples/Roslyn.DeMagic.PackageSmoke/Roslyn.DeMagic.PackageSmoke.csproj"
nuget_config="$repo_root/examples/Roslyn.DeMagic.PackageSmoke/NuGet.config"
artifacts_dir="$repo_root/artifacts/packages"
build_log="$repo_root/artifacts/roslyn-demagic-package-smoke-build.log"
global_package_dir="${HOME}/.nuget/packages/roslyn.demagic/0.1.0"

mkdir -p "$artifacts_dir"
mkdir -p "$(dirname "$build_log")"
rm -rf "$global_package_dir"

dotnet pack "$repo_root/src/Roslyn.DeMagic/Roslyn.DeMagic.csproj" \
  --configuration Release \
  -o "$artifacts_dir" >/dev/null

dotnet restore "$sample_project" --configfile "$nuget_config" --no-cache >/dev/null

if ! dotnet build "$sample_project" --no-restore --nologo --verbosity minimal >"$build_log" 2>&1; then
  cat "$build_log"
  exit 1
fi

python3 - "$manifest_path" "$build_log" <<'PY'
import collections
import json
import pathlib
import re
import sys

manifest_path = pathlib.Path(sys.argv[1])
build_log_path = pathlib.Path(sys.argv[2])
manifest = json.loads(manifest_path.read_text())
build_lines = build_log_path.read_text().splitlines()

diag_re = re.compile(
    r"^(?P<file>.+\.cs)\((?P<line>\d+),(?P<col>\d+)\): "
    r"(?P<severity>warning|error|info) (?P<id>DM00[12]):")

def normalize_file(path: str) -> str:
    normalized = path.replace("\\", "/")
    marker = "/Samples/"
    index = normalized.rfind(marker)
    if index >= 0:
        return "Samples/" + normalized[index + len(marker):]
    return pathlib.Path(normalized).name

observed = []
seen = set()
for line in build_lines:
    match = diag_re.match(line.strip())
    if not match:
        continue
    normalized = {
        "file": normalize_file(match.group("file")),
        "id": match.group("id"),
        "severity": match.group("severity").lower(),
        "raw": line.strip(),
    }
    key = (normalized["file"], normalized["id"], normalized["severity"])
    if key in seen:
        continue
    seen.add(key)
    observed.append(normalized)

expected_counter = collections.Counter(
    (row["file"], row["id"], row["severity"].lower())
    for row in manifest["expectedDiagnostics"]
)
observed_counter = collections.Counter(
    (row["file"], row["id"], row["severity"])
    for row in observed
)

missing = list((expected_counter - observed_counter).elements())
unexpected = list((observed_counter - expected_counter).elements())
clean_files = set(manifest["expectedCleanFiles"])
clean_hits = [row["raw"] for row in observed if row["file"] in clean_files]

if missing or unexpected or clean_hits:
    if missing:
        print("Missing expected diagnostics:")
        for file, diag_id, severity in missing:
            print(f"  - {file} :: {diag_id} :: {severity}")
    if unexpected:
        print("Unexpected diagnostics:")
        for file, diag_id, severity in unexpected:
            print(f"  - {file} :: {diag_id} :: {severity}")
    if clean_hits:
        print("Diagnostics reported for expected-clean files:")
        for line in clean_hits:
            print(f"  - {line}")
    sys.exit(1)

print("Roslyn.DeMagic package validation passed.")
print(f"Validated {sum(expected_counter.values())} expected diagnostics and {len(clean_files)} clean files.")
PY
