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
