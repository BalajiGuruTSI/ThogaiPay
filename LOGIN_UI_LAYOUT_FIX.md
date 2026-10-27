# ThogaiPay login responsive layout update

Updated the active page used by `AppShell`: `src/ThogaiPay.App/Features/Auth/Views/LoginPage.xaml`.

Changes:
- Removed the fixed 550-device-unit logo width that caused horizontal overflow.
- Logo now scales to the available content width and keeps its aspect ratio.
- Security illustration now uses responsive width and a reduced height.
- Tightened vertical spacing and typography to keep the form visually balanced on common phone sizes.
- Disabled horizontal scrolling and clipped the page background layer to the screen.
- Corrected the mobile icon asset name to `mobile_phone_icon_entry.png`.
- Slightly compacted the reusable floating-label entry dimensions.

No build has been run in this environment. Test on the target Android emulator/device and adjust only after checking the rendered screen.
