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
