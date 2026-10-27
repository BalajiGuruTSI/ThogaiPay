# CTA Inventory and Implementation Status

This inventory is based on the current source snapshot. The repository contains a legacy root MAUI project and the documented build target `src/ThogaiPay.App`. CTA component changes are mirrored into both source trees where relevant. It separates reusable component integration from backend-dependent behavior; it is not a claim that every business workflow is production-connected.

## Shared component

- `Controls/CTAButton.xaml` / `.xaml.cs` and `src/ThogaiPay.App/Shared/Controls/Buttons/CTAButton.xaml` / `.xaml.cs`: implemented with strongly typed variants, command/parameter, inline progress, disabled-state handling, accessible metadata and automation identifier.
- `PrimaryActionButton` and `SecondaryActionButton` in the build-target project remain thin obsolete wrappers for source compatibility; new screens should use `CTAButton` with an explicit variant.
- Tokens: `Resources/Styles/Colors.xaml`.
- Legacy auth CTA integrations: Login, OTP verification, PIN setup, PIN unlock, biometric setup.
- Documented build-target auth CTA integration: `Features/Auth/Views/LoginPage.xaml` uses `CTAButton` for OTP initiation and account creation.
- Recent recharge card actions use the shared secondary CTA and navigate to the existing recharge journey. Prefilling the exact saved connection and DTH-specific route remain pending data/route integration.

## Screen/action mapping

| Screen/area | Action | Component/behavior | Status |
|---|---|---|---|
| Login | Continue / request OTP | `CTAButton`, `BrandedPrimary`, `LoginCommand`, inline busy state | Integrated; real OTP depends on configured auth provider |
| OTP verification | Verify and continue | `CTAButton`, `BrandedPrimary`, `VerifyCommand`, inline busy state | Integrated; backend/auth provider dependent |
| PIN setup | Continue | `CTAButton`, `Primary`, `ContinueCommand` | Integrated |
| PIN unlock | Unlock | `CTAButton`, `Primary`, `UnlockCommand` | Integrated |
| PIN unlock | Forgot PIN | `CTAButton`, `Text`, `ForgotPinCommand` | Integrated |
| Biometric setup | Enable biometrics | `CTAButton`, `Primary`, `EnableCommand` | Integrated |
| Biometric setup | Skip for now | `CTAButton`, `Text`, `SkipCommand` | Integrated |
| Home recent recharge cards | Recharge again | `CTAButton`, `Secondary`, routes to current recharge page | Navigation integrated; connection prefill pending |
| Home recent recharge cards | View all | Existing tap action routes to History | Existing |
| Mobile recharge | Select/confirm plan/payment | Existing screen contains mock/static UI in places | Requires full business-flow audit |
| History/Profile/Settings and other existing controls | Various | Existing controls retained unless listed above | Not all migrated or behavior-verified in this pass |

## Remaining verification/integration

- The root project contains both the legacy root MAUI project and a nested `src/ThogaiPay.App` project. Confirm which project is the intended build target and exclude nested source from the root project if needed before claiming a clean build.
- The environment used for this update does not have the `dotnet` CLI, so MAUI compilation and device rendering were not executed.
- Verify all command CanExecute states and loading/error behavior with unit tests once the actual ViewModels and provider configurations are available.
- Payment/recharge operations must remain server-authoritative and must not report success based only on button activation.
