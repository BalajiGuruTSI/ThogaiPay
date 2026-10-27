# Adding a screen

The login screen is organized by feature:

- `Views/Authentication/LoginPage.xaml` owns the visual layout and bindings.
- `Views/Authentication/LoginPage.xaml.cs` is limited to initialization and the language picker event.
- `ViewModels/Authentication/LoginViewModel.cs` owns presentation state and client-side mobile-number validation.
- `MauiProgram.cs` registers the page and ViewModel with dependency injection.
- `AppShell` composes the initial page after the branded splash screen.

Use semantic `DynamicResource` keys for brand-dependent colors and use the brand generator to replace `brandlogo.png`. Do not put API calls or authentication secrets in the page. When the authentication backend is available, inject an Application-layer authentication use case into the ViewModel and only report OTP delivery after the server confirms the request was accepted.

The current Continue action validates the entered number but does not send an OTP. Create Account is a clearly identified placeholder until the registration flow is implemented.


## Feature ownership rule

Create feature pages under `src/ThogaiPay.App/Features/<FeatureName>/Views/` and ViewModels under the matching `ViewModels/` folder. Do not add new feature pages to the legacy top-level `Views/` folder. Put app-wide startup pages under `Shared/Startup/` and reusable controls under `Shared/Controls/`.

Use `PrimaryActionButton` for shared primary actions. Do not copy its visual styling into each page. Bind its `IsBusy` property to the ViewModel's real asynchronous operation state so progress is shown inside the CTA.
