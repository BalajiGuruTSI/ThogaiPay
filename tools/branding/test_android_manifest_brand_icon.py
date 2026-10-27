"""Regression checks for Android manifest branding references."""
from pathlib import Path
import re

ROOT = Path(__file__).resolve().parents[2]
MANIFEST = ROOT / "src" / "ThogaiPay.App" / "Platforms" / "Android" / "AndroidManifest.xml"


def test_manifest_uses_generated_brand_icon_name():
    text = MANIFEST.read_text(encoding="utf-8-sig")
    assert 'android:icon="@mipmap/brandappicon"' in text
    assert "thogaiappicon" not in text


def test_manifest_does_not_pin_package_namespace():
    text = MANIFEST.read_text(encoding="utf-8-sig")
    assert not re.search(r'\\bpackage\\s*=', text)
