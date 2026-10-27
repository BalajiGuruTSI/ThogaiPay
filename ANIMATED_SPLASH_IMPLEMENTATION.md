# Animated ThogaiPay splash

Implemented in the active startup page used by `App.xaml.cs`:
- `src/ThogaiPay.App/Shared/Startup/BrandedSplashPage.xaml`
- `src/ThogaiPay.App/Shared/Startup/BrandedSplashPage.xaml.cs`

Animation sequence: soft brand glow expands, two thin brand rings reveal, small brand-color accents appear, the configured logo lockup fades/scales into place, tagline fades in, a small orange accent expands, then the page fades to the app.

The native Android splash is platform-controlled and static; it now uses a compact navy/orange mark instead of the orange-circle placeholder. The animated reveal occurs in MAUI immediately afterward.

No Android build/device rendering was available in this environment; build and verify on the target device.
