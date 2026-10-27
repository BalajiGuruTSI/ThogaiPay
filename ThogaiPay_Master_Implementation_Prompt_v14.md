# ThogaiPay Master Implementation Prompt — v11 Addendum

## Splash page XAML/code-behind consistency

- The animated splash uses one `BrandedSplashPage.xaml` and exactly one matching `BrandedSplashPage.xaml.cs` partial class.
- The XAML `x:Class` and C# namespace/type must match exactly.
- Every named control referenced in code-behind must exist in the XAML. Never leave stale references such as `BrandName`, `BrandTagline`, or `BrandAccent` when the official logo asset already contains the full wordmark and tagline.
- Animate the complete official brand lockup as one image, preserving aspect ratio; do not overlay duplicate brand name/tagline text.
- Explicitly ensure the page is included as MAUI XAML in the app project and verify `UseMaui`, `Microsoft.Maui.Controls`, and the target MAUI workload are configured.
- Do not diagnose missing MAUI controls solely as missing `using` statements. Check package restore, workload installation, target framework, XAML build action, generated partial output, and project references.
- Add an automated check for stale XAML control references and duplicate page classes.
- Do not claim a successful MAUI build unless `dotnet restore` and `dotnet build` were actually executed successfully.


## LOGIN SCREEN IMPLEMENTATION REQUIREMENT — v12 ADDITION

Implement the login experience from the supplied ThogaiPay mobile login reference as a responsive, native .NET MAUI XAML screen, governed by `ProjectStructureCheck.md`.

Required structure:
- `Views/Authentication/LoginPage.xaml`
- `Views/Authentication/LoginPage.xaml.cs`
- `ViewModels/Authentication/LoginViewModel.cs`

Requirements:
- Match the reference hierarchy: language selector, official brand lockup, security illustration, welcome title/subtitle, mobile number input, primary Continue CTA, divider, Create Account CTA, and trust reassurance.
- Keep all package-specific identity and logo selection in the brand manifest/generator. Use the generated `brandlogo.png`; never hardcode the ThogaiPay logo path to a customer-specific asset.
- Use semantic theme resources and DynamicResource for brand-dependent styling.
- Keep UI code-behind minimal and register the page and ViewModel through dependency injection.
- Validate mobile input locally and visibly. Never claim an OTP was sent or create an account unless a real backend use case confirms the operation.
- Make the layout scrollable, responsive, accessible, and safe for system bars and the on-screen keyboard.
- Wire the branded splash to the Shell and show the login page as the initial unauthenticated screen.
- Add automated static checks for XAML/code-behind pairing, bindings, dependency registrations, and brand logo generation.
- Document which authentication actions are placeholders pending backend agreement.


# V14 IMPLEMENTATION CORRECTION — FEATURE OWNERSHIP AND SHARED PRIMARY CTA

This section is mandatory and supersedes any older conflicting folder examples in this prompt.

## Feature screen placement
- Put every feature-owned screen under `src/ThogaiPay.App/Features/<FeatureName>/Views/`.
- Put its ViewModels under `src/ThogaiPay.App/Features/<FeatureName>/ViewModels/`.
- Authentication uses `Features/Auth/Views/` and `Features/Auth/ViewModels/`.
- Do not create feature screens under a top-level `Views/` folder.
- App-wide startup UI may live under `Shared/Startup/`; shared controls live under `Shared/Controls/`; shared localization, navigation and design-system resources remain centralized.
- When moving a page, update `x:Class`, C# namespace, DI registration, Shell integration, resource references and tests in the same change.

## Reusable CTA
- Implement one reusable `PrimaryActionButton` at `Shared/Controls/Buttons/PrimaryActionButton.xaml` with matching code-behind.
- Expose meaningful bindable properties: `Text`, `BusyText`, `Command`, and `IsBusy`.
- Use semantic `DynamicResource` keys, including `ActionPrimaryBackground`, `ActionPrimaryForeground`, and `BrandSecondary`, so brand generation changes its appearance without feature-screen edits.
- Render the in-progress indicator and busy text inside the CTA itself. While busy, hide the default label and disable repeat activation. Do not create a separate loader for the same action.
- Keep accessible button semantics and a minimum comfortable touch target.
- Only show busy feedback for a genuine asynchronous operation. Do not add artificial delay to imply network progress and never claim OTP delivery before the authentication API confirms it.
- Replace duplicated primary-button styling in feature screens with the shared component where the action has the same semantics.

## Required validation
- Add tests for feature folder ownership, XAML/code-behind matching, DI registration, semantic resource usage and in-CTA busy feedback.
- Ensure active MAUI image assets use valid platform resource names; remove or rename stale duplicates only when they are confirmed unused.
- Confirm central package version declarations match package references.
- Run actual restore/build/test commands when the .NET SDK and MAUI workloads are available. Static XML/Python checks must not be reported as a successful MAUI compilation.
