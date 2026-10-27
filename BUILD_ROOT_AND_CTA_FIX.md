# Build Root and CTA Partial-Class Fix

## Important: build the solution, not the former legacy project

This repository has one supported MAUI build target:
`src/ThogaiPay.App/ThogaiPay.App.csproj`, included in `ThogaiPay.slnx`.

The duplicate legacy `ThogaiPay.csproj` has been removed so Visual Studio cannot
accidentally select a second MAUI project with a different set of controls.

The unified CTA is `src/ThogaiPay.App/Shared/Controls/Buttons/CTAButton.xaml`
and its matching `.xaml.cs`. It exposes the `Variant` property using
`CTAButtonVariant`. Obsolete `PrimaryActionButton` and `SecondaryActionButton`
wrapper classes are intentionally not present; use `CTAButton` and set `Variant`.

## Replace the working copy correctly

Extract this ZIP so `ThogaiPay.slnx` is directly inside your working folder,
for example:

`E:\Projects\ThogaiPay\ThogaiPay.slnx`

Do not extract it into an extra nested `ThogaiPay` directory. Back up local-only
changes before replacing files.

Close Visual Studio, remove `bin` and `obj` under `src/ThogaiPay.App`, then open
`ThogaiPay.slnx` and run:

```powershell
dotnet restore ThogaiPay.slnx
dotnet clean src/ThogaiPay.App/ThogaiPay.App.csproj
dotnet build src/ThogaiPay.App/ThogaiPay.App.csproj -f net10.0-android
```

If `PrimaryActionButton` or `SecondaryActionButton` still appears in a compiler
error after using this exact archive and solution, Visual Studio is compiling
another checkout or stale generated files. Check the full source path shown in
the Error List before editing code again.
