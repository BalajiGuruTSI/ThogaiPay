# Splash logo update v10

- Replaced the standalone symbol with the supplied official wide ThogaiPay logo lockup.
- The asset includes the multicolour symbol, ThogaiPay wordmark, and Tamil tagline.
- The XAML animates the complete lockup as one image with fade, scale, and upward settle.
- The brand generator copies each selected brand's `assets.splashLogo` into `Resources/Images/brandlogo.<extension>` and emits the generated filename in `BrandIdentity.LogoFileName`.
- Sample Partner continues to use its own manifest-selected logo asset.
- The animated XAML no longer overlays duplicate English name/tagline labels.
- Validation executed: 14 Python unit tests passed; XAML XML parsing passed.
- Not verified: .NET MAUI compile or Android device rendering (not available in the build environment).
