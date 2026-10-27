# ThogaiPay CTA Design System

## Purpose

`ThogaiPay.Controls.CTAButton` is the shared action control for the legacy root MAUI project (`ThogaiPay.csproj`). The documented build target is `src/ThogaiPay.App`; it has the same control under `ThogaiPay.App.Shared.Controls.Buttons.CTAButton`. The two projects are separate targets and must not compile each other's source trees. It centralizes command execution, accessibility metadata, variant styling, disabled state, and inline busy feedback. Feature code owns business logic; the control owns presentation only.

## Variants

| Variant | Use | Tokens |
|---|---|---|
| `Primary` | Main action on ordinary forms and flows | `CTAPrimarySolidBrush`, `CTAPrimaryForeground` |
| `BrandedPrimary` | Approved branded login/OTP action with orange lower accent | `CTAPrimaryGradient`, `CTAAccentBrush` |
| `Secondary` | Outlined alternative action | `CTASurfaceBrush`, `CTASecondaryBorderBrush`, `CTASecondaryForeground` |
| `Text` | Low-emphasis action | `CTATransparentBrush`, `CTATextForeground` |
| `Recharge` | Recharge-specific emphasis | `CTARechargeBrush`, `CTARechargeAccentBrush` |
| `Destructive` | Destructive confirmation only | `CTADestructiveBrush`, `CTADestructiveForeground` |

The approved login mockup is the reason `BrandedPrimary` uses a gradient and orange lower accent. Ordinary `Primary` uses solid navy. Colors live in `Resources/Styles/Colors.xaml`; white-label branding should map these semantic keys to generated brand resources rather than set colors on individual screens.

## Example

```xml
<controls:CTAButton
    Text="{Binding ContinueText}"
    BusyText="Sending OTP..."
    Command="{Binding LoginCommand}"
    IsBusy="{Binding IsBusy}"
    Variant="BrandedPrimary"
    TrailingIcon="→"
    AccessibilityLabel="Continue with mobile number"
    AccessibilityHint="Requests a one-time password for the entered mobile number."
    AutomationIdentifier="Login_GetOtp" />
```

## Behavior

- The underlying native `Button` owns command execution and touch semantics.
- `IsBusy=true` disables repeat activation and shows an `ActivityIndicator` plus `BusyText` inside the same CTA.
- `Command`, `CommandParameter`, `IsBusy`, `Variant`, icon text, accessible label/hint and automation identifier are bindable.
- `Clicked` is exposed only for UI navigation actions that are not yet represented by a ViewModel command. Prefer `Command` for feature/business operations.
- The control does not fabricate success, perform HTTP calls, or own payment/recharge logic.
- Use one dominant primary CTA per screen. Keep destructive operations confirmed by the feature flow.

## Accessibility

Supply `AccessibilityLabel` when the visible text is not enough, and `AccessibilityHint` where the outcome is not obvious. Keep the control at least 48 dp for primary form actions. The small recent-recharge CTA uses a compact height only within its card.

## Adding a variant

1. Add a value to `CTAButtonVariant`.
2. Add the semantic brush/foreground keys in `Resources/Styles/Colors.xaml`.
3. Map the variant in `CTAButton.ApplyVariant()`.
4. Add a static validation assertion and verify light/dark theme contrast.
5. Do not add screen-specific hardcoded colors to use the variant.
