#!/usr/bin/env python3
"""Generate a selected brand before invoking the MAUI build."""
from __future__ import annotations
import argparse
import subprocess
import sys
from pathlib import Path

ROOT = Path(__file__).resolve().parents[2]
BRAND_TOOL = ROOT / "tools" / "branding" / "brand.py"
APP_PROJECT = ROOT / "src" / "ThogaiPay.App" / "ThogaiPay.App.csproj"


def main() -> int:
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("--brand", required=True, help="BrandId under branding/brands")
    parser.add_argument("--framework", default="net10.0-android", help="Target framework (default: net10.0-android)")
    parser.add_argument("--configuration", default="Debug", choices=("Debug", "Release"))
    parser.add_argument("--no-restore", action="store_true", help="Pass --no-restore to dotnet build")
    args = parser.parse_args()

    validate = subprocess.run(
        [sys.executable, str(BRAND_TOOL), "validate", "--brand", args.brand], cwd=ROOT
    )
    if validate.returncode:
        return validate.returncode

    generate = subprocess.run(
        [sys.executable, str(BRAND_TOOL), "generate", "--brand", args.brand], cwd=ROOT
    )
    if generate.returncode:
        return generate.returncode

    command = [
        "dotnet", "build", str(APP_PROJECT), "--framework", args.framework,
        "--configuration", args.configuration,
    ]
    if args.no_restore:
        command.append("--no-restore")
    print("Running:", " ".join(command), flush=True)
    return subprocess.run(command, cwd=ROOT).returncode


if __name__ == "__main__":
    raise SystemExit(main())
