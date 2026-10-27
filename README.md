# ThogaiPay

A .NET MAUI application foundation with brand-manifest validation and generated white-label build resources.

## Brand selection drives the build

The selected brand manifest is the source of truth. Its `displayName`, `appId`, colors, assets, and locale metadata generate both native identity properties and semantic design resources. The package identifier is generated from the manifest; it is not a runtime heuristic for guessing the palette.

### Build ThogaiPay

From the repository root:

```bash
python tools/build/build_brand.py --brand thogaipay --framework net10.0-android
```

### Build the sample partner

```bash
python tools/build/build_brand.py --brand sample-partner --framework net10.0-android
```

The wrapper validates the selected manifest, generates resources before MSBuild evaluates the app project, then invokes `dotnet build`. This keeps the package identifier, display name, icon, splash, and semantic colors aligned. The generated files are under `src/ThogaiPay.App/Branding/Generated/`; do not edit them manually.

### Validate/generate without building

```bash
python tools/branding/brand.py validate --brand thogaipay
python tools/branding/brand.py generate --brand thogaipay --dry-run
python tools/branding/brand.py generate --brand thogaipay
python tools/branding/test_branding.py
```

Switching brands changes the generated build identity and compiled resource palette. It does not change the package identity of an already-installed app at runtime. Before creating a release, build using the intended brand explicitly.

## Centralized styling

Feature pages and shared controls should use semantic resources such as `BrandPrimary`, `BrandSecondary`, `TextPrimary`, `SurfaceDefault`, `ActionPrimaryBackground`, and `NavigationSelected`. They should not hardcode brand hex values. `Resources/Styles/Styles.xaml` consumes generated semantic resources; `BrandResources.xaml` is generated from the selected manifest.

## Current status

Python brand validation/generation tests are available. MAUI compilation and device verification must be run on a machine with the pinned .NET SDK and required MAUI workloads. The build wrapper will report the actual build result when run. URL scheme metadata is generated, but platform-specific deep-link registration still requires implementation and verification. Locale metadata does not substitute for a complete `.resx` localization implementation.

See `docs/branding/WHITE_LABEL_GUIDE.md` and `ThogaiPay_Master_Implementation_Prompt_v2.md`.


## XAML resource dictionary and generated asset requirements

The generated brand dictionary and shared style dictionaries are class-backed MAUI `ResourceDictionary` files. Keep each XAML `x:Class` synchronized with its matching partial C# code-behind class and constructor calling `InitializeComponent()`.

Generated asset names must be MAUI-safe. The branding generator uses `brandappicon.*` and `brandsplash.*` (lowercase letters/digits, no hyphens). If Visual Studio reports `Color` or `ResourceDictionary` as not found, first verify the XAML Build Action/MAUI project context and the dictionary `x:Class`/code-behind pairing; do not randomly add package references.

Run the validation checks from the repository root:

```bash
python tools/branding/brand.py validate --brand thogaipay
python tools/branding/brand.py generate --brand thogaipay
python tools/branding/test_branding.py
```

The Python branding tests validate manifest and generated-resource behavior. They do not replace `dotnet build`; compile the MAUI target on a machine with the required .NET SDK and MAUI workloads.


## Authentication login screen

The initial unauthenticated experience is `Views/Authentication/LoginPage.xaml`. It follows the approved mobile login reference using the generated brand lockup, wave background, secure-login illustration, language selector (English/Tamil), mobile input, Continue action, account-creation action, and trust reassurance.

The screen and `LoginViewModel` are registered in `MauiProgram.cs`. Continue validates a 10-digit Indian mobile number but does not send an OTP; real authentication and account creation must be connected to approved Application-layer use cases and the backend before release.

Run static project checks:

```bash
python -m pytest -q tools/branding tools/validation
python tools/branding/brand.py validate --brand thogaipay
```


## Feature and shared UI ownership

The active MAUI application is `src/ThogaiPay.App` (the solution references this project). Feature screens live under `Features/<FeatureName>/Views` and their ViewModels live under `Features/<FeatureName>/ViewModels`. App-wide startup UI belongs under `Shared/Startup`; reusable visual controls belong under `Shared/Controls`; design tokens and shared styles remain in the centralized design system. Do not add new feature pages under `Views/`.

The login page is `src/ThogaiPay.App/Features/Auth/Views/LoginPage.xaml`. Its reusable primary CTA is `src/ThogaiPay.App/Shared/Controls/Buttons/PrimaryActionButton.xaml`. The CTA consumes semantic DynamicResource keys generated from the selected brand and shows its ActivityIndicator and busy text inside the button surface. Do not add a separate page-level loader for this CTA.

A busy indicator should only appear while a real asynchronous operation is in progress. The current login CTA performs local validation only, so it does not simulate an OTP request or artificial loading delay. When the authentication API is integrated, bind the ViewModel's busy state to `PrimaryActionButton.IsBusy`.

The repository root also contains a legacy project snapshot. For the current architecture and build workflow, use `ThogaiPay.slnx` and the `src/ThogaiPay.App` project rather than the legacy root-level `ThogaiPay.csproj`.


## Unified CTA system

The shared CTA implementation and screen inventory are documented in:

- `docs/cta/CTA_DESIGN_SYSTEM.md`
- `docs/cta/CTA_SCREEN_INVENTORY.md`
- `docs/cta/CTA_BUILD_VALIDATION.md`

Use `CTAButton` with an explicit variant (`Primary`, `BrandedPrimary`, `Secondary`, `Text`, `Recharge`, or `Destructive`) for new actions. All brand-facing CTA colors are centralized in the relevant `Resources/Styles/Colors.xaml`; loading progress is rendered inside the CTA and repeat activation is blocked while busy. `BrandedPrimary` is reserved for approved gradient-plus-orange-accent flows such as the login mockup; standard primary actions use solid navy.
