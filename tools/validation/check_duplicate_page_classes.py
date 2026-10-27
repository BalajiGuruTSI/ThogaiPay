"""Detect duplicate hand-written BrandedSplashPage declarations in C# sources."""
from pathlib import Path
import re
ROOT = Path(__file__).resolve().parents[2]
APP = ROOT / "src" / "ThogaiPay.App"
pattern = re.compile(r"\b(?:public\s+)?partial\s+class\s+BrandedSplashPage\b|\bclass\s+BrandedSplashPage\b")
matches = []
for path in APP.rglob("*.cs"):
    text = path.read_text(encoding="utf-8-sig", errors="ignore")
    if pattern.search(text):
        matches.append(path.relative_to(ROOT).as_posix())
if len(matches) != 1:
    raise SystemExit(f"Expected exactly one hand-written BrandedSplashPage declaration; found {len(matches)}: {matches}")
print(f"PASS: single BrandedSplashPage code-behind declaration: {matches[0]}")
