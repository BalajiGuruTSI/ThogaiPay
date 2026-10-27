# ThogaiPay White-Label Branding Guide

## Source of truth

Each brand is defined by `branding/brands/<brandId>/brand.json`. The manifest owns the display name, application ID, URL scheme metadata, colors, assets, locale settings, and permitted client feature flags. The application ID is generated from the selected manifest; it must not be independently hardcoded elsewhere.

ThogaiPay's canonical palette is navy `#101F50`, sunrise orange `#F59E0B`, green `#16A34A`, white `#FFFFFF`, blue `#2563EB`, and cyan `#06B6D4`.

## Easiest build workflow

Run from the repository root:

```bash
python tools/build/build_brand.py --brand thogaipay --framework net10.0-android
```

For the sample partner:

```bash
python tools/build/build_brand.py --brand sample-partner --framework net10.0-android
```

The build wrapper validates the manifest, generates resources before the MAUI project is evaluated, and invokes `dotnet build`. It ensures that the selected display name, package ID, icon, splash, and semantic palette originate from the same brand configuration.

For validation and generator tests without a .NET build:

```bash
python tools/branding/brand.py validate --brand thogaipay
python tools/branding/brand.py generate --brand thogaipay --dry-run
python tools/branding/test_branding.py
```

## Generated files

Only declared outputs under `src/ThogaiPay.App/Branding/Generated/` are written:

- `Brand.Generated.props`: display title, application ID, BrandId, locale metadata, and selected native icon/splash items.
- `BrandResources.xaml`: semantic light/dark resource mappings and compatibility keys.
- `brandappicon.*` and `brandsplash.*`: selected assets copied from manifest paths.

Do not hand-edit generated files. Update the manifest or source assets and regenerate. Generation is designed to update declared output files only; it does not remove unrelated files.

## How styling changes

The selected manifest generates semantic resources. Shared styles consume those semantic keys, so the same screen XAML can be used for either brand. Do not use the package name to construct colors or write customer-specific conditional logic in pages. The package ID is a build output and consistency check; the manifest is the authoritative configuration.

The selected brand palette is compiled into the app. Light/dark theme resources should preserve that brand's identity and readable contrast. Native package IDs, app icons, and splash screens are build-time properties and cannot be changed by editing runtime JSON in an installed app.

## Limitations to verify

- `urlScheme` is validated and emitted as build metadata. Platform-specific URL registration must be implemented and verified for Android/iOS before claiming deep-link support.
- Locale settings are metadata; actual language support requires real `.resx` resources and runtime language switching.
- Feature flags are not security authorization.
- A successful Python validation is not evidence of a successful MAUI build. Run the build wrapper on a machine with the required .NET SDK and workloads.


## XAML resource dictionary compilation

The generated `BrandResources.xaml` and shared `Resources/Styles/Styles.xaml` are class-backed MAUI `ResourceDictionary` files. Each declares a matching `x:Class` and has a partial C# code-behind class that calls `InitializeComponent()`. Keep these names and namespaces synchronized if either dictionary is renamed. This allows MAUI's XAML compiler/source generator to resolve `Color`, `ResourceDictionary`, and other MAUI XAML types correctly.

Generated asset filenames use lowercase letters and digits only (for example, `brandappicon.png` and `brandsplash.svg`) to comply with MAUI resource naming rules. Do not introduce hyphens into generated resource filenames.


### App icon safe zone and centering

The app icon source must place the foreground mark centrally with transparent padding around it. Keep the artwork within approximately 70–75% of the square canvas to allow Android launcher masks to crop safely. Do not bake rounded-square or circular launcher masks into the foreground artwork. The ThogaiPay source asset uses a centered 1024×1024 transparent canvas. After changing an icon, regenerate the selected brand and uninstall/reinstall the debug app if Android caches the old launcher icon.

Icon asset validation uses Pillow. Install the optional branding test dependency with `python -m pip install -r tools/branding/requirements.txt` before running `python tools/branding/test_icon_safe_zone.py`.


### Launcher icon background and centering (updated)

For ThogaiPay, the app icon source is an opaque white 1024×1024 square with the logo optically centered and safely padded. This avoids transparent areas being rendered black by some launcher/icon paths. Keep the foreground artwork around 68–72% of the canvas. If you later switch to a native adaptive icon with separate foreground/background layers, explicitly configure the background as white rather than relying on transparency. Reinstall the app after changing icons because Android may cache launcher assets.


## Branded animated splash screen

The app uses two layers:
1. The native MAUI splash (`MauiSplashScreen`) for the operating-system startup handoff. Native splash screens are intentionally static.
2. `Views/Startup/BrandedSplashPage.cs`, a short in-app brand reveal that animates the selected logo, brand name, and tagline before navigating to `AppShell`.

The brand manifest now defines `tagline` and `assets.splashLogo`. The branding generator emits `BrandIdentity.g.cs` and copies the selected logo to `Resources/Images/brandlogo.<extension>`. The generated identity includes the logo filename, so the splash page does not contain a customer-specific asset name.

Change the manifest, then run:
```bash
python tools/branding/brand.py validate --brand thogaipay
python tools/branding/brand.py generate --brand thogaipay
```
Use `--brand sample-partner` for the sample partner. `tools/build/build_brand.py` validates and generates the selected brand before building. Keep the reveal brief (about two seconds), honor reduced-motion/accessibility preferences when adding more animation, and never delay access to the app for decorative animation.


### XAML splash and supplied background image

The in-app animated splash is `src/ThogaiPay.App/Views/Startup/BrandedSplashPage.xaml` with animation orchestration in its code-behind. The supplied blue/orange waves image is copied to `Resources/Images/splashbackground.jpg` and displayed edge-to-edge. Brand name, tagline, logo, and accent color are read from generated `BrandIdentity`. The native OS launch screen is a separate required platform handoff; it is configured with a white background to reduce the visual jump before the full-screen XAML splash.


### Full-lockup splash logo

The ThogaiPay splash uses the approved wide logo artwork that already contains the symbol, `ThogaiPay` wordmark, and Tamil tagline. The selected brand manifest's `assets.splashLogo` path is copied by `tools/branding/brand.py` to `Resources/Images/brandlogo.<extension>`, and `BrandIdentity.LogoFileName` is generated alongside it. The splash animates this single lockup as one unit to avoid duplicating the name or tagline. For another customer, provide that customer's own approved lockup asset and update only that brand manifest; do not edit the splash XAML. Preserve the source artwork's aspect ratio and use `AspectFit`.
