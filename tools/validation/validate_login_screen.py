"""Static checks for feature-owned login UI and reusable primary action."""
from pathlib import Path
import xml.etree.ElementTree as ET
import re

ROOT = Path(__file__).resolve().parents[2]
APP = ROOT / "src" / "ThogaiPay.App"
LOGIN = APP / "Features" / "Auth" / "Views" / "LoginPage.xaml"
LOGIN_CS = APP / "Features" / "Auth" / "Views" / "LoginPage.xaml.cs"
VIEWMODEL = APP / "Features" / "Auth" / "ViewModels" / "LoginViewModel.cs"
CTA = APP / "Shared" / "Controls" / "Buttons" / "PrimaryActionButton.xaml"
CTA_CS = APP / "Shared" / "Controls" / "Buttons" / "PrimaryActionButton.xaml.cs"

def test_login_xaml_is_well_formed():
    ET.parse(LOGIN)

def test_login_xaml_codebehind_class_matches():
    xaml = LOGIN.read_text(encoding="utf-8")
    code = LOGIN_CS.read_text(encoding="utf-8")
    assert 'x:Class="ThogaiPay.App.Features.Auth.Views.LoginPage"' in xaml
    assert "namespace ThogaiPay.App.Features.Auth.Views;" in code
    assert "partial class LoginPage : ContentPage" in code

def test_login_bindings_exist_on_viewmodel():
    xaml = LOGIN.read_text(encoding="utf-8")
    vm = VIEWMODEL.read_text(encoding="utf-8")
    for name in ("MobileNumber", "Message", "HasMessage", "MessageColor",
                 "ContinueCommand", "CreateAccountCommand", "IsContinuing",
                 "ContinueBusyText", "PrivacyNoteText"):
        assert name in xaml
        assert name in vm

def test_login_page_and_viewmodel_are_registered_for_di():
    source = (APP / "MauiProgram.cs").read_text(encoding="utf-8")
    assert "AddTransient<LoginViewModel>" in source
    assert "AddTransient<LoginPage>" in source
    shell = (APP / "AppShell.xaml.cs").read_text(encoding="utf-8")
    assert "LoginPage loginPage" in shell

def test_login_uses_generated_brand_logo_and_semantic_colors():
    xaml = LOGIN.read_text(encoding="utf-8")
    assert 'Source="brandlogo.png"' in xaml
    assert "{DynamicResource BrandPrimary}" in xaml
    assert "<buttons:PrimaryActionButton" in xaml
    assert "BackgroundColor=\"#101F50\"" not in xaml

def test_primary_action_is_shared_and_has_inline_busy_feedback():
    ET.parse(CTA)
    xaml = CTA.read_text(encoding="utf-8")
    code = CTA_CS.read_text(encoding="utf-8")
    assert "ActivityIndicator" in xaml
    assert "IsBusy" in code
    assert "ActionPrimaryBackground" in xaml
    assert "ActionPrimaryForeground" in xaml
    assert "IsBusy=\"{Binding IsContinuing}\"" in LOGIN.read_text(encoding="utf-8")

def test_active_feature_screens_are_not_under_legacy_views_folder():
    assert not (APP / "Views" / "Authentication" / "LoginPage.xaml").exists()
    assert LOGIN.exists()


def test_active_android_image_resource_names_are_safe():
    image_dir = APP / "Resources" / "Images"
    for path in image_dir.iterdir():
        if path.is_file():
            assert re.fullmatch(r"[a-z][a-z0-9_]*\.[a-z0-9]+", path.name), path.name
