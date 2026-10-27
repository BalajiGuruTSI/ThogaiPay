"""Regression tests for brand-specific full-lockup splash artwork."""
import importlib.util
import unittest
from pathlib import Path

ROOT = Path(__file__).resolve().parents[2]
SCRIPT = ROOT / "tools" / "branding" / "brand.py"
SPEC = importlib.util.spec_from_file_location("brand_tool_splash_test", SCRIPT)
brand = importlib.util.module_from_spec(SPEC)
assert SPEC.loader is not None
SPEC.loader.exec_module(brand)

class SplashBrandLockupTests(unittest.TestCase):
    def test_thogaipay_manifest_uses_supplied_full_brand_lockup(self):
        _, manifest = brand.load_manifest("thogaipay")
        self.assertEqual(
            "branding/brands/thogaipay/assets/thogaipay_brand_lockup.png",
            manifest["assets"]["splashLogo"],
        )
        self.assertTrue((ROOT / manifest["assets"]["splashLogo"]).is_file())

    def test_splash_uses_generated_brand_logo_and_animates_single_lockup(self):
        xaml = (ROOT / "src/ThogaiPay.App/Views/Startup/BrandedSplashPage.xaml").read_text(encoding="utf-8")
        code = (ROOT / "src/ThogaiPay.App/Views/Startup/BrandedSplashPage.xaml.cs").read_text(encoding="utf-8")
        identity = (ROOT / "src/ThogaiPay.App/Branding/Generated/BrandIdentity.g.cs").read_text(encoding="utf-8")
        self.assertIn('x:Name="BrandLogo"', xaml)
        self.assertIn('BrandIdentity.LogoFileName', code)
        self.assertIn('BrandLogo.FadeTo', code)
        self.assertIn('BrandLogo.ScaleTo', code)
        self.assertIn('LogoFileName = "brandlogo.png"', identity)
        self.assertNotIn('x:Name="BrandName"', xaml)
        self.assertNotIn('x:Name="BrandTagline"', xaml)

if __name__ == "__main__":
    unittest.main()
