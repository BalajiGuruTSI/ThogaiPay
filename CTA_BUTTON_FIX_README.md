# CTA button implementation fix

## Root cause
`PrimaryActionButton.xaml` and `SecondaryActionButton.xaml` declared XAML-generated partial classes inheriting `ContentView`, while their C# classes inherited `CTAButton`. C# partial declarations cannot have different base classes. The generated `ContentView` classes also do not expose `CTAButton.Variant`, which explains the `Variant` error.

## Implementation change
- Removed the two conflicting XAML files.
- Kept `PrimaryActionButton` and `SecondaryActionButton` as code-only compatibility aliases over `CTAButton`.
- `PrimaryActionButton` sets `CTAButtonVariant.BrandedPrimary`.
- `SecondaryActionButton` sets `CTAButtonVariant.Secondary`.
- `CTAButton.xaml` / `CTAButton.xaml.cs` remain the single XAML-backed implementation for rendering, commands, busy state, accessibility, and variants.

Do not recreate `PrimaryActionButton.xaml` or `SecondaryActionButton.xaml` unless their implementation is changed to a proper standalone ContentView rather than a subclass of CTAButton.
