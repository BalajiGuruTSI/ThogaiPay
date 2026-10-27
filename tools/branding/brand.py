#!/usr/bin/env python3
"""Validate a ThogaiPay brand manifest and generate MAUI brand resources safely."""
from __future__ import annotations
import argparse, json, re, shutil, sys
from pathlib import Path
import xml.etree.ElementTree as ET

ROOT = Path(__file__).resolve().parents[2]
APP = ROOT / 'src' / 'ThogaiPay.App'
BRANDS = ROOT / 'branding' / 'brands'
GENERATED = APP / 'Branding' / 'Generated'
HEX = re.compile(r'^#[0-9A-Fa-f]{6}$')
APP_ID = re.compile(r'^[A-Za-z][A-Za-z0-9_]*(\.[A-Za-z][A-Za-z0-9_]*)+$')
SCHEME = re.compile(r'^[a-z][a-z0-9+.-]*$')

REQUIRED_COLORS = {
    'primary': '#101F50',
    'secondary': '#F59E0B',
    'success': '#16A34A',
    'surface': '#FFFFFF',
    'accentBlue': '#2563EB',
    'accentCyan': '#06B6D4',
}

NEUTRALS = {
    'Black': '#000000', 'White': '#FFFFFF', 'Gray100': '#F3F4F6',
    'Gray200': '#E5E7EB', 'Gray300': '#D1D5DB', 'Gray400': '#9CA3AF',
    'Gray500': '#6B7280', 'Gray600': '#4B5563', 'Gray700': '#374151',
    'Gray800': '#1F2937', 'Gray900': '#111827', 'Gray950': '#030712',
    'OffBlack': '#101318'
}


def load_manifest(brand_id: str) -> tuple[Path, dict]:
    path = BRANDS / brand_id / 'brand.json'
    if not path.is_file():
        raise ValueError(f'No manifest found for BrandId={brand_id!r}: {path.relative_to(ROOT)}')
    data = json.loads(path.read_text(encoding='utf-8'))
    return path, data


def validate(brand_id: str, data: dict) -> list[str]:
    errors: list[str] = []
    for field in ('brandId', 'displayName', 'appId', 'urlScheme', 'assets', 'colors', 'localization'):
        if field not in data:
            errors.append(f'Missing required field: {field}')
    if data.get('brandId') != brand_id:
        errors.append(f"Manifest brandId {data.get('brandId')!r} does not match selected BrandId {brand_id!r}")
    if not isinstance(data.get('displayName'), str) or not data.get('displayName', '').strip():
        errors.append('displayName must be a non-empty string')
    if not isinstance(data.get('tagline'), str) or not data.get('tagline', '').strip():
        errors.append('tagline must be a non-empty string')
    if not APP_ID.fullmatch(str(data.get('appId', ''))):
        errors.append('appId must be a reverse-domain identifier, e.g. in.example.wallet')
    if not SCHEME.fullmatch(str(data.get('urlScheme', ''))):
        errors.append('urlScheme must be lowercase and contain only letters, digits, +, . or -')
    colors = data.get('colors', {})
    for key in REQUIRED_COLORS:
        value = colors.get(key)
        if not isinstance(value, str) or not HEX.fullmatch(value):
            errors.append(f'colors.{key} must be a six-digit hex color')
    for key, expected in REQUIRED_COLORS.items():
        if brand_id == 'thogaipay' and colors.get(key, '').upper() != expected:
            errors.append(f'ThogaiPay canonical color colors.{key} must be {expected}')
    assets = data.get('assets', {})
    for key in ('appIcon', 'splash', 'splashLogo'):
        relative = assets.get(key)
        if not isinstance(relative, str) or not relative.strip():
            errors.append(f'assets.{key} must be a project-relative path')
            continue
        asset_path = (ROOT / relative).resolve()
        if ROOT.resolve() not in asset_path.parents:
            errors.append(f'assets.{key} must remain inside the repository')
        elif not asset_path.is_file():
            errors.append(f'assets.{key} file does not exist: {relative}')
    localization = data.get('localization', {})
    if not localization.get('defaultLocale') or not isinstance(localization.get('supportedLocales'), list) or not localization.get('supportedLocales'):
        errors.append('localization must define defaultLocale and a non-empty supportedLocales array')
    elif localization['defaultLocale'] not in localization['supportedLocales']:
        errors.append('defaultLocale must be included in supportedLocales')
    if not isinstance(data.get('features', {}), dict):
        errors.append('features must be an object of boolean feature flags')
    elif any(not isinstance(v, bool) for v in data['features'].values()):
        errors.append('every features value must be boolean')
    return errors


def color_resource(name: str, value: str) -> str:
    return f'    <Color x:Key="{name}">{value}</Color>'


def generate(brand_id: str, dry_run: bool = False) -> list[str]:
    manifest_path, data = load_manifest(brand_id)
    errors = validate(brand_id, data)
    if errors:
        raise ValueError('\n'.join(errors))

    colors = {k: v.upper() for k, v in data['colors'].items()}
    primary, secondary, success = colors['primary'], colors['secondary'], colors['success']
    surface = colors['surface']
    # Semantic mappings are brand-owned; neutral values remain centrally governed.
    text_primary = '#101F50' if brand_id == 'thogaipay' else '#172033'
    surface_secondary = '#F3F6FC' if brand_id == 'thogaipay' else '#F1F5F9'
    # Dark mappings preserve the selected brand identity while maintaining readable surfaces.
    dark_primary = '#93C5FD' if brand_id == 'thogaipay' else '#67E8F9'
    dark_surface = '#0B1220'
    dark_surface_elevated = '#111C2E'
    dark_text_primary = '#F8FAFC'
    resources = {
        'BrandPrimary': primary, 'BrandSecondary': secondary,
        'BrandAccent': colors['accentBlue'], 'BrandSuccess': success,
        'BrandAccentCyan': colors['accentCyan'],
        'SurfaceDefault': surface, 'SurfaceElevated': surface,
        'SurfaceSecondary': surface_secondary,
        'BorderDefault': '#D8DFEA', 'DividerDefault': '#E7EBF2',
        'TextPrimary': text_primary,
        'DarkBrandPrimary': dark_primary, 'DarkBrandSecondary': colors['accentCyan'],
        'DarkBrandAccent': '#93C5FD', 'DarkBrandSuccess': '#4ADE80',
        'DarkSurfaceDefault': dark_surface, 'DarkSurfaceElevated': dark_surface_elevated,
        'DarkSurfaceSecondary': '#172338', 'DarkBorderDefault': '#334155',
        'DarkDividerDefault': '#273449', 'DarkTextPrimary': dark_text_primary,
        'DarkTextSecondary': '#CBD5E1', 'DarkTextPlaceholder': '#94A3B8',
        'TextSecondary': '#5C6680', 'TextSubtitle': '#697386',
        'TextDisabled': '#9CA3AF', 'TextPlaceholder': '#7B8498',
        'TextLabel': '#34405E',
        'ActionPrimaryBackground': primary,
        'ActionPrimaryForeground': '#FFFFFF',
        'ActionPrimaryDisabledBackground': '#AAB2C5',
        'ActionPrimaryDisabledForeground': '#F8FAFC',
        'ActionSecondaryBackground': secondary,
        'ActionSecondaryForeground': text_primary,
        'DarkActionPrimaryBackground': dark_primary,
        'DarkActionPrimaryForeground': '#0B1220',
        'DarkActionSecondaryBackground': '#26364D',
        'DarkActionSecondaryForeground': '#F8FAFC',
        'InputBackground': surface, 'InputBorder': '#CBD5E1',
        'InputFocusedBorder': colors['accentBlue'], 'InputErrorBorder': '#DC2626',
        'InputFloatingLabel': '#5C6680', 'InputPlaceholder': '#7B8498',
        'StatusSuccess': success, 'StatusWarning': '#B45309',
        'StatusError': '#DC2626', 'StatusInformation': colors['accentBlue'],
        'NavigationBackground': surface, 'NavigationSelected': primary,
        'NavigationUnselected': '#697386', 'NavigationIndicator': secondary,
        'DarkNavigationBackground': dark_surface, 'DarkNavigationSelected': dark_primary,
        'DarkNavigationUnselected': '#94A3B8', 'DarkNavigationIndicator': colors['accentCyan'],
        # Compatibility keys used by the default MAUI styles.
        'Primary': primary, 'PrimaryDark': dark_primary, 'PrimaryDarkText': '#FFFFFF', 'MidnightBlue': primary,
        'Secondary': secondary, 'SecondaryDarkText': '#FFFFFF',
        'Tertiary': colors['accentCyan'], 'Magenta': colors['accentBlue'],
        'White': '#FFFFFF', 'Black': '#000000',
    }
    resources.update(NEUTRALS)
    xaml = '''<?xml version="1.0" encoding="UTF-8" ?>\n<ResourceDictionary xmlns="http://schemas.microsoft.com/dotnet/2021/maui" xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml" x:Class="ThogaiPay.App.Branding.Generated.BrandResources">\n'''
    xaml += '\n'.join(color_resource(k, v) for k, v in resources.items())
    xaml += '\n</ResourceDictionary>\n'

    app_icon_source = (ROOT / data['assets']['appIcon']).resolve()
    splash_source = (ROOT / data['assets']['splash']).resolve()
    splash_logo_source = (ROOT / data['assets']['splashLogo']).resolve()
    splash_logo_name = 'brandlogo' + splash_logo_source.suffix.lower()
    icon_ext = app_icon_source.suffix.lower()
    splash_ext = splash_source.suffix.lower()
    icon_name = 'brand_app_icon' + icon_ext
    splash_name = 'brand_splash' + splash_ext
    props = f'''<Project>\n  <!-- Generated by tools/branding/brand.py. Do not edit by hand. -->\n  <PropertyGroup>\n    <ApplicationTitle>{data['displayName']}</ApplicationTitle>\n    <ApplicationId>{data['appId']}</ApplicationId>\n    <BrandId>{data['brandId']}</BrandId>\n    <BrandUrlScheme>{data['urlScheme']}</BrandUrlScheme>\n    <BrandSplashColor>#FFFFFF</BrandSplashColor>\n    <BrandDefaultLocale>{data['localization']['defaultLocale']}</BrandDefaultLocale>\n    <BrandSupportedLocales>{';'.join(data['localization']['supportedLocales'])}</BrandSupportedLocales>\n  </PropertyGroup>\n  <ItemGroup>\n    <MauiIcon Remove="Resources\\AppIcon\\thogaiappicon.png" />\n    <MauiIcon Remove="Resources\\AppIcon\\appicon.svg" />\n    <MauiIcon Include="Branding\\Generated\\{icon_name}" />\n    <MauiSplashScreen Remove="Resources\\Splash\\splash.svg" />\n    <MauiSplashScreen Include="Branding\\Generated\\{splash_name}" Color="$(BrandSplashColor)" BaseSize="128,128" />\n  </ItemGroup>\n</Project>\n'''
    identity_code = f'''// Generated by tools/branding/brand.py. Do not edit by hand.
namespace ThogaiPay.App.Branding.Generated;

public static class BrandIdentity
{{
    public const string BrandId = "{data['brandId']}";
    public const string DisplayName = "{data['displayName'].replace(chr(34), chr(92)+chr(34))}";
    public const string Tagline = "{data.get('tagline', '').replace(chr(34), chr(92)+chr(34))}";
    public const string LogoFileName = "{splash_logo_name}";
    public const string PrimaryColor = "{primary}";
    public const string SecondaryColor = "{secondary}";
}}
'''

    resource_codebehind = '''// Generated by tools/branding/brand.py. Do not edit by hand.
using Microsoft.Maui.Controls;

namespace ThogaiPay.App.Branding.Generated;

public partial class BrandResources : ResourceDictionary
{
    public BrandResources()
    {
        InitializeComponent();
    }
}
'''
    outputs = {
        GENERATED / 'BrandResources.xaml': xaml,
        GENERATED / 'BrandResources.xaml.cs': resource_codebehind,
        GENERATED / 'Brand.Generated.props': props,
        GENERATED / 'BrandIdentity.g.cs': identity_code,
        APP / 'Resources' / 'Images' / splash_logo_name: splash_logo_source,
        GENERATED / icon_name: app_icon_source,
        GENERATED / splash_name: splash_source,
    }
    if dry_run:
        print(f'Brand: {brand_id} ({data["displayName"]})')
        print(f'ApplicationId: {data["appId"]}')
        for path, content in outputs.items():
            print(f'Would generate: {path.relative_to(ROOT)}')
        return [str(p.relative_to(ROOT)) for p in outputs]

    GENERATED.mkdir(parents=True, exist_ok=True)
    changed = []
    for destination, content in outputs.items():
        if isinstance(content, Path):
            same = destination.is_file() and destination.read_bytes() == content.read_bytes()
            if not same:
                shutil.copy2(content, destination)
                changed.append(str(destination.relative_to(ROOT)))
        else:
            encoded = content.encode('utf-8')
            if not destination.is_file() or destination.read_bytes() != encoded:
                destination.write_bytes(encoded)
                changed.append(str(destination.relative_to(ROOT)))

    # Remove only stale generated logo files in the reserved brandlogo.* namespace.
    active_names = {path.name for path in outputs}
    for stale_logo in (APP / 'Resources' / 'Images').glob('brandlogo.*'):
        if stale_logo.is_file() and stale_logo.name != splash_logo_name:
            stale_logo.unlink()
            changed.append(str(stale_logo.relative_to(ROOT)) + ' (stale generated logo removed)')
    for stale in GENERATED.iterdir():
        if (stale.is_file()
                and re.fullmatch(r'(brandappicon|brandsplash)\.(png|svg)', stale.name)
                and stale.name not in active_names):
            stale.unlink()
            changed.append(str(stale.relative_to(ROOT)) + ' (stale generated asset removed)')

    print(f'Validated brand: {brand_id} ({data["displayName"]})')
    print(f'ApplicationId: {data["appId"]}')
    print('Generated/updated: ' + (', '.join(changed) if changed else 'no changes (deterministic output)'))
    return changed


def main() -> int:
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument('command', choices=('validate', 'generate'))
    parser.add_argument('--brand', required=True, help='BrandId folder under branding/brands')
    parser.add_argument('--dry-run', action='store_true', help='Validate and list outputs without writing files')
    args = parser.parse_args()
    try:
        _, manifest = load_manifest(args.brand)
        errors = validate(args.brand, manifest)
        if errors:
            print('Brand validation failed:', file=sys.stderr)
            for error in errors:
                print(f' - {error}', file=sys.stderr)
            return 1
        if args.command == 'validate':
            print(f'Brand validation passed: {args.brand} ({manifest["displayName"]})')
            print(f'ApplicationId: {manifest["appId"]}')
            return 0
        generate(args.brand, dry_run=args.dry_run)
        return 0
    except (OSError, json.JSONDecodeError, ValueError) as exc:
        print(f'Brand tooling error: {exc}', file=sys.stderr)
        return 1

if __name__ == '__main__':
    raise SystemExit(main())
