"""Static regression checks for MAUI restore and Android resource naming."""
from pathlib import Path
import re
import xml.etree.ElementTree as ET

ROOT = Path(__file__).resolve().parents[2]
APP = ROOT / "src" / "ThogaiPay.App"


def test_app_central_package_versions_cover_references():
    props = ET.parse(APP / "Directory.Packages.props").getroot()
    csproj = ET.parse(APP / "ThogaiPay.App.csproj").getroot()
    versions = {item.attrib["Include"].lower() for item in props.findall(".//PackageVersion")}
    refs = {item.attrib["Include"].lower() for item in csproj.findall(".//PackageReference")}
    assert refs <= versions, f"Missing PackageVersion entries: {sorted(refs - versions)}"


def test_maui_resource_filenames_have_no_hyphens_or_invalid_characters():
    resource_roots = [APP / "Resources" / "Images", APP / "Resources" / "AppIcon"]
    invalid = []
    for folder in resource_roots:
        if not folder.exists():
            continue
        for path in folder.rglob("*"):
            if path.is_file() and not re.fullmatch(r"[A-Za-z0-9_.]+", path.name):
                invalid.append(str(path.relative_to(ROOT)))
    assert not invalid, f"Invalid MAUI resource filenames: {invalid}"


def test_splash_and_login_reference_renamed_assets():
    splash = (APP / "Shared" / "Startup" / "BrandedSplashPage.xaml").read_text(encoding="utf-8")
    login = (APP / "Features" / "Auth" / "Views" / "LoginPage.xaml").read_text(encoding="utf-8")
    assert "splashbackground.jpg" in splash and "splashbackground.jpg" in login
    assert "loginsecurityillustration.svg" in login
