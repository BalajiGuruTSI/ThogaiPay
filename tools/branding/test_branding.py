"""Lightweight tests for brand manifest validation and generated resources."""
import importlib.util
import unittest
from pathlib import Path

SCRIPT = Path(__file__).with_name('brand.py')
SPEC = importlib.util.spec_from_file_location('thogaipay_brand_tool', SCRIPT)
brand = importlib.util.module_from_spec(SPEC)
assert SPEC.loader is not None
SPEC.loader.exec_module(brand)

class BrandManifestTests(unittest.TestCase):
    @classmethod
    def setUpClass(cls):
        # Keep tests deterministic regardless of which brand was generated last.
        brand.generate('thogaipay')

    def test_thogaipay_manifest_is_valid_and_uses_canonical_palette(self):
        _, manifest = brand.load_manifest('thogaipay')
        self.assertEqual([], brand.validate('thogaipay', manifest))
        for key, value in brand.REQUIRED_COLORS.items():
            self.assertEqual(value, manifest['colors'][key].upper())

    def test_sample_partner_manifest_is_valid(self):
        _, manifest = brand.load_manifest('sample-partner')
        self.assertEqual([], brand.validate('sample-partner', manifest))
        self.assertNotEqual('in.thogaipay.app', manifest['appId'])

    def test_manifest_identity_mismatch_is_rejected(self):
        _, manifest = brand.load_manifest('thogaipay')
        self.assertTrue(any('does not match selected BrandId' in issue for issue in brand.validate('sample-partner', manifest)))

    def test_required_assets_exist_for_each_brand(self):
        for brand_id in ('thogaipay', 'sample-partner'):
            _, manifest = brand.load_manifest(brand_id)
            for asset in manifest['assets'].values():
                self.assertTrue((brand.ROOT / asset).is_file(), f'{brand_id}: missing {asset}')


    def test_generated_resources_match_selected_thogaipay_identity(self):
        resources = (brand.APP / 'Branding' / 'Generated' / 'BrandResources.xaml').read_text(encoding='utf-8')
        props = (brand.APP / 'Branding' / 'Generated' / 'Brand.Generated.props').read_text(encoding='utf-8')
        self.assertIn('x:Class="ThogaiPay.App.Branding.Generated.BrandResources"', resources)
        self.assertIn('x:Key="BrandPrimary">#101F50</Color>', resources)
        self.assertIn('x:Key="BrandSecondary">#F59E0B</Color>', resources)
        self.assertIn('x:Key="DarkSurfaceDefault">#0B1220</Color>', resources)
        self.assertIn('<ApplicationTitle>ThogaiPay</ApplicationTitle>', props)
        self.assertIn('<ApplicationId>in.thogaipay.app</ApplicationId>', props)

    def test_shared_styles_use_generated_semantic_brand_resources(self):
        styles = (brand.APP / 'Resources' / 'Styles' / 'Styles.xaml').read_text(encoding='utf-8-sig')
        self.assertIn('{StaticResource BrandPrimary}', styles)
        self.assertIn('{StaticResource DarkBrandPrimary}', styles)
        self.assertNotIn('{StaticResource Primary}', styles)


    def test_partner_generation_switches_identity_and_palette_then_restores_default(self):
        try:
            brand.generate('sample-partner')
            resources = (brand.APP / 'Branding' / 'Generated' / 'BrandResources.xaml').read_text(encoding='utf-8')
            props = (brand.APP / 'Branding' / 'Generated' / 'Brand.Generated.props').read_text(encoding='utf-8')
            self.assertIn('x:Class="ThogaiPay.App.Branding.Generated.BrandResources"', resources)
            self.assertIn('x:Key="BrandPrimary">#164E63</Color>', resources)
            self.assertIn('<ApplicationTitle>Sample Partner Wallet</ApplicationTitle>', props)
            self.assertIn('<ApplicationId>in.samplepartner.wallet</ApplicationId>', props)
        finally:
            brand.generate('thogaipay')
        generated = brand.APP / 'Branding' / 'Generated'
        self.assertTrue((generated / 'brandappicon.png').is_file())
        self.assertFalse((generated / 'brandappicon.svg').exists())

    def test_brand_identity_includes_name_tagline_and_logo_filename(self):
        identity = (brand.APP / 'Branding' / 'Generated' / 'BrandIdentity.g.cs').read_text(encoding='utf-8')
        self.assertIn('DisplayName = "ThogaiPay"', identity)
        self.assertIn('Tagline = "Every Payment. One Simple Touch."', identity)
        self.assertIn('LogoFileName = "brandlogo.png"', identity)

    def test_splash_page_uses_generated_brand_identity_not_hardcoded_brand(self):
        page = (brand.APP / 'Views' / 'Startup' / 'BrandedSplashPage.xaml.cs').read_text(encoding='utf-8')
        self.assertIn('BrandIdentity.DisplayName', page)
        self.assertIn('complete brand lockup image', page)
        self.assertIn('BrandIdentity.LogoFileName', page)
        self.assertNotIn('Text = "ThogaiPay"', page)

    def test_resource_dictionaries_are_class_backed(self):
        brand_xaml = (brand.APP / 'Branding' / 'Generated' / 'BrandResources.xaml').read_text(encoding='utf-8')
        brand_cs = (brand.APP / 'Branding' / 'Generated' / 'BrandResources.xaml.cs').read_text(encoding='utf-8')
        styles_xaml = (brand.APP / 'Resources' / 'Styles' / 'Styles.xaml').read_text(encoding='utf-8-sig')
        styles_cs = (brand.APP / 'Resources' / 'Styles' / 'Styles.xaml.cs').read_text(encoding='utf-8')
        colors_xaml = (brand.APP / 'Resources' / 'Styles' / 'Colors.xaml').read_text(encoding='utf-8-sig')
        colors_cs = (brand.APP / 'Resources' / 'Styles' / 'Colors.xaml.cs').read_text(encoding='utf-8')
        self.assertIn('x:Class="ThogaiPay.App.Branding.Generated.BrandResources"', brand_xaml)
        self.assertIn('partial class BrandResources : ResourceDictionary', brand_cs)
        self.assertIn('x:Class="ThogaiPay.App.Resources.Styles.Styles"', styles_xaml)
        self.assertIn('partial class Styles : ResourceDictionary', styles_cs)
        self.assertIn('x:Class="ThogaiPay.App.Resources.Styles.Colors"', colors_xaml)
        self.assertIn('partial class Colors : ResourceDictionary', colors_cs)

    def test_generated_maui_asset_names_are_valid(self):
        _, manifest = brand.load_manifest('thogaipay')
        brand.generate('thogaipay')
        props = (brand.APP / 'Branding' / 'Generated' / 'Brand.Generated.props').read_text(encoding='utf-8')
        self.assertIn('Branding\\Generated\\brandappicon.png', props)
        self.assertIn('Branding\\Generated\\brandsplash.svg', props)
        for path in (brand.APP / 'Branding' / 'Generated').iterdir():
            if path.suffix.lower() in {'.png', '.svg'} and path.name.startswith('brand'):
                self.assertRegex(path.name, r'^[a-z][a-z0-9_]*\.(png|svg)$')

    def test_app_project_does_not_hardcode_brand_identity(self):
        project = (brand.APP / 'ThogaiPay.App.csproj').read_text(encoding='utf-8-sig')
        self.assertNotIn('<ApplicationTitle>ThogaiPay</ApplicationTitle>', project)
        self.assertNotIn('<ApplicationId>in.thogaipay.app</ApplicationId>', project)
        self.assertIn('Branding\\Generated\\Brand.Generated.props', project)

if __name__ == '__main__':
    unittest.main(verbosity=2)
