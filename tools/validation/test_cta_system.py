from pathlib import Path
import xml.etree.ElementTree as ET

ROOT = Path(__file__).resolve().parents[2]
ACTIVE_APP = ROOT / "src" / "ThogaiPay.App"

def test_cta_xaml_parses_in_both_project_trees():
    ET.parse(ROOT / "Controls" / "CTAButton.xaml")
    ET.parse(ACTIVE_APP / "Shared" / "Controls" / "Buttons" / "CTAButton.xaml")

def test_cta_component_has_all_variants_in_both_project_trees():
    for path in (
        ROOT / "Controls" / "CTAButton.xaml.cs",
        ACTIVE_APP / "Shared" / "Controls" / "Buttons" / "CTAButton.xaml.cs",
    ):
        text = path.read_text(encoding="utf-8")
        for variant in ("Primary", "BrandedPrimary", "Secondary", "Text", "Recharge", "Destructive"):
            assert variant in text

def test_cta_loading_is_inline():
    for path in (
        ROOT / "Controls" / "CTAButton.xaml",
        ACTIVE_APP / "Shared" / "Controls" / "Buttons" / "CTAButton.xaml",
    ):
        xaml = path.read_text(encoding="utf-8")
        assert "ActivityIndicator" in xaml
        assert "BusyText" in xaml
        assert "IsBusy" in xaml

def test_brand_tokens_are_centralized():
    for path in (
        ROOT / "Resources" / "Styles" / "Colors.xaml",
        ACTIVE_APP / "Resources" / "Styles" / "Colors.xaml",
    ):
        text = path.read_text(encoding="utf-8-sig")
        for key in ("CTAPrimarySolidBrush", "CTAPrimaryGradient", "CTAAccentBrush",
                    "CTASecondaryBorderBrush", "CTARechargeBrush", "CTADestructiveBrush"):
            assert key in text

def test_auth_pages_use_shared_cta():
    for path in (
        ROOT / "Views" / "Authentication" / "LoginPage.xaml",
        ROOT / "Views" / "Authentication" / "OtpVerificationPage.xaml",
        ROOT / "Views" / "Authentication" / "PinSetupPage.xaml",
        ROOT / "Views" / "Authentication" / "PinUnlockPage.xaml",
        ROOT / "Views" / "Authentication" / "BiometricSetupPage.xaml",
        ACTIVE_APP / "Features" / "Auth" / "Views" / "LoginPage.xaml",
    ):
        assert "CTAButton" in path.read_text(encoding="utf-8"), str(path)
