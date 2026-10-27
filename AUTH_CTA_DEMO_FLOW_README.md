# Login CTA + static OTP demo flow

Changes are based on the uploaded `ThogaiPay(4).zip`.

- Login CTA now uses the shared `CTAButton` busy state. The busy spinner and `ContinueBusyText` are rendered inside the CTA.
- Valid 10-digit mobile numbers start a short progress state and navigate to the OTP page.
- Added `Features/Auth/Views/OtpVerificationPage.xaml` and code-behind.
- Demo OTP is `123456`; this flow does not send SMS and must be replaced by the real authentication API before production.
- OTP verification CTA also shows its progress inside the button.
- BrandedPrimary CTA foreground is explicitly white to prevent the dark text seen in the screenshot; gradient and orange lower accent are set directly in code.
- Registered the OTP route in AppShell.

No full .NET MAUI build or Android device run was performed in this environment.
