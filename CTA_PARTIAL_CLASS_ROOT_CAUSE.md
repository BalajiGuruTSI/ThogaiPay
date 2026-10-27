# CTA partial-class error: root cause and fix

The errors were caused by a mismatch between each wrapper's XAML and C# base class:

- `PrimaryActionButton.xaml` declared an XAML-generated partial class deriving from `ContentView`.
- `PrimaryActionButton.xaml.cs` declared `PrimaryActionButton : CTAButton`.
- `SecondaryActionButton` had the same mismatch.

A XAML-backed partial class must use a consistent base class in generated and handwritten declarations. These wrappers are intentionally code-only compatibility subclasses of `CTAButton`, so the two wrapper `.xaml` files were removed while their `.xaml.cs` files were retained.

`CTAButton` remains the only XAML-backed shared CTA and defines the `Variant` bindable property and `CTAButtonVariant` enum. This removes the generated partial classes that caused the base-class conflicts and the cascading `Variant` errors.

After extracting this project, close Visual Studio, delete `bin` and `obj` folders, open `ThogaiPay.slnx`, and build `src/ThogaiPay.App/ThogaiPay.App.csproj`. Compilation must be verified on the local .NET MAUI SDK.
