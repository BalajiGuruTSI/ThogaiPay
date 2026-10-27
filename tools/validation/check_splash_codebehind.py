"""Check that splash code-behind references only named XAML controls."""
from pathlib import Path
import re

ROOT = Path(__file__).resolve().parents[2]
XAML = ROOT / "src/ThogaiPay.App/Views/Startup/BrandedSplashPage.xaml"
CS = ROOT / "src/ThogaiPay.App/Views/Startup/BrandedSplashPage.xaml.cs"


def test_codebehind_control_references_exist_in_xaml():
    xaml = XAML.read_text(encoding="utf-8")
    cs = CS.read_text(encoding="utf-8")
    names = set(re.findall(r'x:Name="([A-Za-z_][A-Za-z0-9_]*)"', xaml))
    refs = set(re.findall(r'\b(?:BrandLogo|SplashRoot|BrandName|BrandTagline|BrandAccent)\b', cs))
    refs -= {"BrandLogo", "SplashRoot"} if False else set()
    # Only named controls (not generated BrandIdentity properties) need to exist in XAML.
    controls = {name for name in refs if name in {"BrandLogo", "SplashRoot", "BrandName", "BrandTagline", "BrandAccent"}}
    assert controls <= names, f"Code-behind references missing XAML names: {sorted(controls - names)}"
    assert "BrandName" not in cs
    assert "BrandTagline" not in cs
    assert "BrandAccent" not in cs


def test_single_class_and_single_animation_state():
    cs = CS.read_text(encoding="utf-8")
    assert cs.count("class BrandedSplashPage") == 1
    assert cs.count("_continueToApp") == 3
    assert cs.count("_hasStarted") == 3
    assert cs.count("OnAppearing()") == 1
