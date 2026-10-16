# ThogaiPay — Complete .NET MAUI Application Development Master Prompt

> **Mandatory design-reference rule:** Never implement or redesign a screen from memory. First locate and inspect its approved reference, screen-specific MockUI and Assets folders, and asset manifest. Use the approved reference as the visual source of truth. If the required reference or asset is missing, document the gap rather than inventing a replacement. After implementation, compare the actual rendered UI with the reference, correct mismatches, and record verification results.

## 1. ROLE AND RESPONSIBILITY

Act as a **Principal .NET MAUI Architect, Senior C# Developer, UI/UX Engineer, Mobile Application Specialist, QA Engineer and Technical Lead**.

Build a complete, modern, professional, fast and responsive **ThogaiPay B2C mobile and desktop application** using .NET MAUI.

The application must support:

- Android
- iOS
- Windows desktop
- Mac Catalyst desktop

This is a **new, independent B2C application**. Do not refer to, reuse, or depend on the source code, backend implementation, database, authentication, or business logic of the existing B2B application.

The backend API is not ready. Build a fully navigable, functional application using mock data and mock services, with clean interfaces that allow real APIs to be integrated later without rewriting the UI or business logic.

Do not stop after generating a plan or a few sample screens. Inspect the repository, establish the architecture, implement the application incrementally, run builds and tests, and document the results.

When correcting an existing screen, modify the existing implementation incrementally instead of rebuilding a working screen from scratch unless a rewrite is explicitly required by the user.

---

## 2. MANDATORY DESIGN REFERENCE

The project contains a design reference package named:

`ThogaiPay_MockUI_and_Assets`

The developer may place it under:

`DesignReference/ThogaiPay_MockUI_and_Assets/`

First, inspect the actual repository and locate this folder. Do not assume its location or rename its existing folders unnecessarily.

The package contains screen-specific `MockUI` and `Assets` folders, shared logo and image assets, design tokens, and an implementation prompt.

### Required workflow for every screen

Before implementing a screen:

1. Locate its matching folder in `ThogaiPay_MockUI_and_Assets`.
2. Inspect the `MockUI` folder and its mockup.
3. Read the screen's design notes, if present.
4. Read its `Assets/asset-manifest.json`.
5. Inspect the referenced shared assets and design tokens.
6. Copy or adapt the required assets into the appropriate MAUI resource folders.
7. Implement the screen to closely match the reference.
8. Compare the rendered screen against the reference and refine spacing, typography, sizing, colours, card shapes, navigation and alignment.
9. Build and test the affected platform before marking the screen complete.

For example:

- `01_LoginPage/MockUI/` — login screen reference.
- `01_LoginPage/Assets/` — login screen asset manifest.
- `03_HomePage/MockUI/` — home screen reference.
- `03_HomePage/Assets/` — home screen asset manifest.
- `04_MobileRecharge/MockUI/` — mobile recharge reference.
- `04_MobileRecharge/Assets/` — mobile recharge asset manifest.

Use the actual folder names in the package if they differ slightly from these examples.

### Asset handling rules

- Copy the required image and icon files into `Resources/Images/`.
- Use the appropriate MAUI resource configuration for fonts and other assets.
- Preserve descriptive filenames and organise assets into sensible subfolders where appropriate.
- Use SVG assets through a compatible MAUI image pipeline, or convert them to appropriate raster formats when necessary.
- Verify that SVG files render correctly on every supported platform.
- Never reference temporary local paths outside the application project.
- Do not use the entire mockup SVG as a live application screen. Recreate it using native MAUI controls and reusable components.
- Do not invent substitute artwork when the correct reference asset exists.
- If an asset is missing, document the gap and create a consistent temporary placeholder instead of silently changing the design.

The mockup is the visual source of truth. The actual application must remain interactive, accessible and functional.

---

## 3. APP BRANDING — FIRST PRIORITY

Start with the application icon and splash screen before developing other pages.

### App icon

Use the supplied approved logo asset:

`thogai_logo`

Inspect the supplied file and confirm its format and dimensions.

Configure the appropriate MAUI application icon resources and generate the platform-specific icon variants required by Android, iOS, Windows and Mac Catalyst.

Requirements:

- Use the actual ThogaiPay brand mark.
- Do not add an unintended background, text or border.
- Preserve aspect ratio and transparent areas where the platform permits them.
- Check that the icon remains recognisable at small sizes.
- Do not replace the supplied logo with a generic icon or emoji.

If `thogai_logo` is not present, search the shared logo assets and report the missing asset. Do not silently substitute a different design.

### Splash screen

Use the supplied image:

`thogai_splash_screen`

The splash screen must display the supplied artwork as a **full-screen visual experience**, rather than showing the image as a small logo centred on a plain background.

Requirements:

- Preserve the composition and aspect ratio of the artwork.
- Fill the available screen area with an appropriate crop or cover behaviour.
- Avoid stretching or distorting the image.
- Account for different aspect ratios, safe areas and screen sizes.
- Avoid showing an unintended default MAUI splash screen as the primary branded experience.
- Configure the platform launch presentation appropriately, then transition into a custom full-screen MAUI splash page when necessary.
- Keep the splash duration short and deterministic.
- Do not introduce an artificial delay when the application is already ready.
- Navigate to the correct initial page after startup.
- Respect the platform lifecycle and handle startup failures safely.

**Platform limitation:** Android 12 and later enforce a system-managed launch splash, and iOS also has native launch-screen requirements. Configure these launch surfaces to match the ThogaiPay branding as closely as the platform allows, then display the full-screen custom splash artwork. Do not promise that the operating system's native launch mechanism can be removed entirely.

### Asset naming

Use the supplied names as the source of truth. If platform-specific naming conventions require additional generated files, preserve the original source assets and document the generated variants.

Do not begin implementing the rest of the app until the icon, launch presentation and custom splash transition have been configured and verified.

---

## 4. REQUIRED TECHNOLOGY STACK

Use the following stack unless a genuine compatibility issue requires a documented alternative.

- .NET MAUI with C# and XAML.
- MVVM architecture.
- CommunityToolkit.Mvvm for observable properties and commands, where compatible with the selected .NET version.
- Shell navigation or a well-structured navigation abstraction.
- Dependency injection using the built-in .NET service container.
- `HttpClient` through `IHttpClientFactory` or the appropriate MAUI-compatible registration pattern.
- `System.Text.Json` for API serialization.
- xUnit for unit testing.
- SQLite only if local persistence is needed for mock/demo data or offline requirements.
- Native MAUI controls and reusable custom controls for UI implementation.

Do not add libraries without a clear reason. Check compatibility with the installed .NET SDK and the target platform versions before adding NuGet packages.

Do not introduce Entity Framework, a backend database dependency, or a real payment integration for this frontend-only phase.

---

## 5. SCALABLE SOLUTION ARCHITECTURE

Use a neat, maintainable architecture with clear separation of responsibilities.

Recommended solution structure:

```text
ThogaiPay.sln
│
├── src/
│   ├── ThogaiPay.App/
│   │   ├── App.xaml
│   │   ├── App.xaml.cs
│   │   ├── MauiProgram.cs
│   │   ├── AppShell.xaml
│   │   ├── AppShell.xaml.cs
│   │   │
│   │   ├── Resources/
│   │   │   ├── AppIcon/
│   │   │   ├── Splash/
│   │   │   ├── Images/
│   │   │   ├── Fonts/
│   │   │   ├── Styles/
│   │   │   └── Raw/
│   │   │
│   │   ├── Controls/
│   │   ├── Converters/
│   │   ├── Behaviors/
│   │   ├── Localization/
│   │   ├── Navigation/
│   │   ├── Platforms/
│   │   └── AppStartup/
│   │
│   ├── ThogaiPay.Core/
│   │   ├── Models/
│   │   ├── DTOs/
│   │   ├── Interfaces/
│   │   ├── Enums/
│   │   ├── Validation/
│   │   └── Constants/
│   │
│   ├── ThogaiPay.Features/
│   │   ├── Authentication/
│   │   ├── Home/
│   │   ├── Recharge/
│   │   ├── History/
│   │   ├── Favourites/
│   │   ├── Profile/
│   │   ├── Settings/
│   │   └── Support/
│   │
│   └── ThogaiPay.Infrastructure/
│       ├── Api/
│       ├── MockServices/
│       ├── Repositories/
│       ├── LocalStorage/
│       ├── Configuration/
│       ├── FeatureFlags/
│       └── Logging/
│
└── tests/
    ├── ThogaiPay.Core.Tests/
    ├── ThogaiPay.Features.Tests/
    └── ThogaiPay.Infrastructure.Tests/
```

Adapt this structure to the existing repository if one already exists. Avoid creating unnecessary projects or folders solely to match this example.

### Architecture rules

- Views contain UI markup and minimal view-specific code.
- ViewModels contain presentation state and commands.
- Business rules must not be implemented directly in XAML code-behind.
- Services encapsulate authentication, recharge, history, favourites, profile, support and configuration operations.
- Interfaces belong in an appropriate abstraction layer.
- Mock implementations and future API implementations must satisfy the same interfaces.
- Repositories or data-access abstractions must not be coupled to individual page implementations.
- Use dependency injection consistently.
- Do not use service locators or static global mutable state as substitutes for proper dependency injection.
- Do not duplicate business logic across ViewModels.
- Keep navigation, error handling, loading state, localisation and theme selection reusable.

Create a short `ARCHITECTURE.md` describing project responsibilities, dependencies, navigation and data flow.

---

## 6. COMPLETE APPLICATION SCREEN FLOW

Implement the screens below in the order described.

### Stage A — Startup and authentication

#### 1. App icon and splash screen

Configure `thogai_logo` and `thogai_splash_screen`, as described above.

Startup flow:

1. Platform launch presentation.
2. Custom branded splash page.
3. Load local configuration and preferences.
4. Initialise the feature-flag provider.
5. Check the configured authentication/session state.
6. Navigate to Login when the user is not authenticated.
7. Navigate to Home only when a valid authenticated session exists.
8. Display a recoverable error if required initialisation fails.

Do not fabricate a valid login session.

#### 2. Login page

Reference: `01_LoginPage`.

UI elements:

- ThogaiPay logo and wordmark.
- Tamil Nadu-inspired artwork from the asset package.
- Tamil/English language selector.
- Welcome heading and concise supporting text.
- Mobile number input with country code `+91`.
- Primary `Send OTP` action.
- Optional Google sign-in only if the corresponding authentication capability is enabled and implemented.
- Clear validation messages.
- Secure sign-in reassurance.
- Loading and error states.

Behaviour:

- Accept a valid Indian mobile number.
- Reject invalid input with a clear message.
- Disable duplicate submissions while the request is running.
- Use the mock authentication service for this phase.
- Navigate to OTP Verification only after the mock service returns a successful result.
- Do not use a hardcoded production OTP.
- Keep the authentication service interface ready for the future API.

#### 3. OTP verification

Reference: `02_OTPVerification`.

UI elements:

- Back navigation.
- Masked mobile number.
- Edit-number action.
- Six-digit OTP input.
- Resend countdown.
- `Verify & Continue` button.
- Loading, invalid-code, expired-code and network-error states.

Behaviour:

- Support entering digits, moving focus, pasting a code and deleting digits.
- Validate the expected code length.
- Simulate verification through a mock authentication service.
- Handle expired or incorrect codes without crashing.
- Prevent repeated submissions while verification is in progress.
- Navigate to Home only after the mock service reports success.

Store no real credentials in source code. Keep the mock authentication implementation explicitly marked as development-only.

---

### Stage B — Home and recharge

#### 4. Home page

Reference: `03_HomePage`.

UI elements:

- Tamil-inspired ThogaiPay branded header.
- Greeting and profile shortcut.
- Quick Recharge card.
- Mobile/DTH selector.
- Quick Favourites.
- Add New Favourite shortcut.
- Recent Recharges.
- Modern bottom tab bar.

Behaviour:

- Load favourite connections and recent transactions from mock services.
- Allow the user to open Mobile Recharge or DTH Recharge.
- Allow the user to select a saved favourite and open the corresponding recharge flow with the saved number or subscriber ID and operator prefilled.
- Allow the user to add a new favourite.
- Allow the user to view recharge history.
- Provide empty states when no favourites or transactions exist.
- Refresh data through pull-to-refresh or an appropriate explicit refresh action where useful.

Do not show wallet balance, Add Money, QR scanning or money transfer.

#### 5. Mobile Recharge

Reference: `04_MobileRecharge`.

UI elements:

- Mobile number input.
- Operator selection.
- Recommended plan categories.
- Recharge plan cards.
- Plan validity, data and benefit information.
- Amount selection.
- Recharge action.
- Appropriate loading and error states.

Behaviour:

1. Enter or prefill a mobile number.
2. Validate the number.
3. Select an operator.
4. Load available mock plans.
5. Select a plan or permitted custom amount.
6. Review the selected number, operator and amount.
7. Invoke a mock recharge service.
8. Display a result through the agreed recharge flow.
9. Make the result available in transaction history.

Keep all operator, plan and transaction data behind interfaces so the future API can replace the mock implementation.

Do not claim a real recharge was completed. The UI must clearly distinguish simulated transactions from live transactions during development.

#### 6. DTH Recharge

Reference: `05_DTHRecharge`.

UI elements:

- Subscriber ID or Smart Card Number input.
- DTH operator selection.
- Plan categories and mock plan cards.
- Amount and plan details.
- Recharge action.

Behaviour:

Follow the same overall structure, card styles, spacing, interaction patterns and error handling as Mobile Recharge.

Only the input fields, operator list, validation rules and DTH-specific plan data should differ.

**The Mobile Recharge and DTH Recharge pages must not introduce inconsistent layouts or an extra Mobile/DTH tab on only one page.** Keep the design language consistent with the approved mockups.

#### 7. Recharge result and transaction details

Reference: `07_TransactionDetails`.

The result returned by the mock recharge service must be represented clearly. The user must be able to open the transaction record and inspect:

- Transaction/reference ID.
- Recharge type.
- Operator.
- Masked or appropriately displayed destination number.
- Amount.
- Timestamp.
- Status.
- Plan details when available.
- Help/support action.

Use explicit simulated status values such as `Success`, `Failed` or `Pending` when supplied by the mock service.

Do not invent a successful outcome independently of the service result.

This is a transaction-detail view, not a separate payment-confirmation screen.

---

### Stage C — History and favourites

#### 8. Recharge History

Reference: `06_RechargeHistory`.

UI elements:

- Search field.
- Modern filter button opening a bottom sheet or filter panel.
- Mobile/DTH filters.
- Status filters.
- Date-range selection.
- Transaction list with amount, operator, date and status.
- Clear filters action.
- Empty, loading and error states.

Behaviour:

- Search by supported fields such as destination number, operator and reference ID.
- Combine filters correctly.
- Allow the user to reset filters.
- Display stable ordering and appropriate date formatting.
- Open Transaction Details when a transaction is selected.
- Keep filtering logic unit-testable.
- Use mock data that includes different service types and statuses.

Do not hardcode filter results in the UI.

#### 9. Favourites

Reference: `08_Favourites`.

UI elements:

- Search saved connections.
- Mobile and DTH favourites.
- Display name and masked destination identifier.
- Operator.
- Quick recharge action.
- Edit and delete actions.
- Add New Favourite action.

Behaviour:

- Load favourites from the mock repository.
- Allow add, edit and delete operations.
- Persist demo favourites locally if appropriate.
- Ask for confirmation before deleting.
- Prefill the relevant recharge screen when a favourite is selected.
- Require the user to review the recharge amount and explicitly initiate the recharge.

#### 10. Add Favourite type selection

Reference: `09_AddFavouriteType`.

Offer two choices:

- Mobile Number.
- DTH Connection.

Navigate to the appropriate form.

#### 11. Add Mobile Favourite

Reference: `10_AddMobileFavourite`.

Fields and behaviour:

- Mobile number.
- Operator selection.
- Optional preferred recharge amount.
- Input validation.
- Duplicate handling.
- Continue to name and save.

#### 12. Add DTH Favourite

Reference: `11_AddDTHFavourite`.

Fields and behaviour:

- Subscriber ID.
- DTH operator.
- Optional preferred amount.
- Input validation.
- Duplicate handling.
- Continue to name and save.

#### 13. Favourite name and save

Reference: `12_FavouriteNameAndSave`.

Fields and behaviour:

- Display name.
- Optional avatar.
- Optional default amount.
- Save Favourite action.
- Validation and loading state.

#### 14. Favourite saved

Reference: `13_FavouriteSaved`.

Show the saved favourite summary and provide:

- Recharge Now.
- Add Another.
- Return to Favourites or Home.

Only show success after the mock repository confirms the save operation.

---

### Stage D — Profile and support

#### 15. Profile and Settings

Reference: `14_ProfileSettings`.

Include only consumer-oriented features:

- Personal information.
- My Favourites.
- My Transactions.
- Security and Privacy.
- Help and Support.
- App Preferences.
- Language selection.
- Theme selection.
- Logout.

Payment methods must only be shown if they are part of the approved consumer product scope. Do not add a wallet balance or Add Money option.

Never expose distributor, retailer, dealer, commission, B2B wallet or My Users features.

#### 16. Help and Support

Reference: `15_HelpSupport`.

Include:

- Help Centre.
- Contact Support.
- Report an Issue.
- Feedback.

Use mock support content and local mock actions until the backend is ready. Do not claim that a support ticket has been submitted to a real support system unless the service returns success.

#### 17. Help Centre

Reference: `16_HelpCentre`.

Include:

- Searchable FAQ list.
- Mobile recharge questions.
- DTH recharge questions.
- Favourites guidance.
- Transaction history guidance.
- Authentication and troubleshooting guidance.

Keep FAQ content in a repository or data source rather than embedding all content in the view.

#### 18. Contact Support and Report an Issue

References: `17_ContactSupport` and `18_ReportIssue`.

Include:

- Supported contact methods.
- Clearly documented mock support hours where needed.
- Issue category.
- Description.
- Optional transaction reference.
- Optional attachment only if the capability is implemented.
- Validation, submission state and error feedback.

Do not display unsupported phone numbers, email addresses or operational promises as real data.

---

## 7. NAVIGATION DESIGN

Use a clear and predictable navigation model.

### Main bottom navigation

Use the approved modern floating tab-bar design with five positions:

1. Home.
2. Recharge.
3. Central ThogaiPay brand mark.
4. History.
5. Profile.

Rules:

- The central position must use the actual ThogaiPay logo.
- It must not use a QR, transfer or unsupported-service icon.
- The centre action should return to Home or perform another explicitly approved brand-specific action.
- The selected tab must have a clear visual indicator.
- Maintain consistent icon sizing, labels, spacing, touch targets and active states.
- Ensure the navigation remains usable on different device sizes.
- Do not duplicate the bottom tab bar on Login, OTP or other authentication-only screens.
- Hide or adapt the bar on appropriate full-screen flows such as splash and authentication.

If Shell's default tab bar cannot achieve the approved visual design cleanly, implement a reusable custom navigation component backed by a single navigation state and proper route handling.

Do not duplicate navigation logic across pages.

### Navigation correctness

- Back navigation must preserve reasonable form state when appropriate.
- Prevent accidental duplicate route pushes.
- Handle logout and authentication expiration safely.
- Do not allow unauthenticated users to enter protected pages.
- Ensure Android back-button behaviour is correct.
- Support platform-appropriate navigation on iOS and desktop.
- Avoid navigation loops after splash or login.

---

## 8. REUSABLE UI COMPONENTS

Build reusable components and styles instead of copying controls between pages.

At minimum, evaluate and implement reusable versions of:

- `PrimaryButton`
- `SecondaryButton`
- `IconButton`
- `AppTextField`
- `PhoneNumberEntry`
- `SearchEntry`
- `OtpInput`
- `AppCard`
- `SectionHeader`
- `OperatorCard`
- `RechargePlanCard`
- `TransactionCard`
- `FavouriteCard`
- `StatusBadge`
- `FilterChip`
- `FilterBottomSheet`
- `LoadingIndicator`
- `SkeletonView`
- `EmptyStateView`
- `ErrorStateView`
- `AppHeader`
- `FloatingBottomNavigation`
- `LanguageSelector`
- `ThemeSelector`
- `ConfirmationDialog`

Requirements:

- Use clear binding APIs.
- Support validation and accessibility.
- Keep styling consistent across platforms.
- Avoid excessive custom renderers or platform-specific hacks.
- Use `Border` and modern MAUI layouts where appropriate rather than relying on deprecated or unsuitable controls.
- Avoid deeply nested layouts that harm rendering performance.
- Make controls reusable without adding unnecessary abstraction.

Do not create a separate custom component for every individual instance of a simple control. Reuse abstractions where they genuinely improve maintainability.

---

## 9. CENTRALIZED STYLING AND DESIGN SYSTEM

All colours, typography, spacing, corner radii, elevations, control heights and common states must be defined centrally.

Use the design tokens from the supplied reference package.

Create appropriate resource dictionaries, for example:

- `Colors.xaml`
- `Typography.xaml`
- `Spacing.xaml`
- `ControlStyles.xaml`
- `LightTheme.xaml`
- `DarkTheme.xaml`

Adapt names to the existing project conventions.

Requirements:

- One consistent style for text inputs.
- One consistent style for primary buttons.
- One consistent style for secondary buttons.
- Standardised cards and list rows.
- Standardised error, success, warning and disabled states.
- Standardised spacing and typography.
- Shared styles for mobile and desktop with platform-aware adjustments.
- Avoid repeating hardcoded colours in individual pages.
- Use the design reference palette as the baseline; do not randomly redesign individual screens.

Use the Tamil Nadu-inspired artwork in a restrained, professional manner. Maintain clear contrast and make sure decorative backgrounds do not compromise readability.

---

## 10. THEME SELECTION

Support at least:

- System Default.
- Light Theme.
- Dark Theme.

Requirements:

- Store the user's selected theme locally.
- Apply it consistently to all screens and reusable controls.
- Restore the selection during startup.
- Avoid flashing the wrong theme when the application opens.
- Provide readable text, fields, icons, borders and status colours in both themes.
- Do not simply invert every colour mechanically.
- Ensure the dark theme remains consistent with the ThogaiPay brand.
- Test theme changes on Android, iOS and desktop.

Theme switching should be handled centrally through a theme service or equivalent abstraction.

---

## 11. LANGUAGE AND LOCALISATION

Support:

- English.
- Tamil.

Requirements:

- Store all user-facing strings in resource files.
- Use strongly typed or otherwise maintainable resource access.
- Provide Tamil and English resource dictionaries/files.
- Allow language selection from the relevant authentication and profile/settings screens.
- Persist the selected language.
- Apply language changes without requiring unnecessary application restarts.
- Update headings, button labels, errors, empty states, validation messages, dialogs and accessibility descriptions.
- Ensure Tamil Unicode renders correctly.
- Choose fonts that support Tamil glyphs on every target platform.
- Avoid hardcoded English strings inside XAML and ViewModels.
- Use culturally appropriate Tamil translations rather than literal or misleading translations.

Document any platform limitation that prevents an immediate language update.

---

## 12. FEATURE FLAGS — REQUIRED DESIGN

Every independently switchable application feature must have a feature flag with a default value of `true`.

Examples:

- Login.
- OTP authentication.
- Google sign-in.
- Home.
- Mobile recharge.
- DTH recharge.
- Recharge plans.
- Transaction history.
- Transaction details.
- Favourites.
- Add Favourite.
- Profile.
- Help Centre.
- Contact Support.
- Report an Issue.
- Theme selection.
- Language selection.
- Optional animations.

Use a central feature-flag abstraction such as:

`IFeatureFlagService`

Suggested responsibilities:

- Read the current value of a flag.
- Return a safe default.
- Refresh flags from a remote configuration source.
- Notify interested components when a flag changes, where appropriate.
- Distinguish between local defaults, remotely supplied values and last-known-good values.

### Configuration priority

Use the following precedence:

1. Valid remote/database configuration.
2. Last-known-good locally cached configuration, if available.
3. Built-in default value of `true`.

The application must remain usable if the feature-flag endpoint is unavailable.

Do not make the application depend on a successful network request just to display the login screen.

### Database/API requirement

The backend is not ready, so do not pretend that a real database exists.

Prepare the client contract for a future feature-flag endpoint and use a mock configuration provider for now.

A possible future response contract is:

```json
{
  "flags": [
    {
      "key": "Feature.MobileRecharge",
      "enabled": true
    },
    {
      "key": "Feature.DTHRecharge",
      "enabled": true
    },
    {
      "key": "Feature.Favourites",
      "enabled": true
    }
  ]
}
```

Document the proposed backend/database structure for the feature flags, including:

- Unique key.
- Enabled state.
- Optional platform or environment scope.
- Optional description.
- Updated timestamp.
- Optional rollout or version information.

Do not implement database administration screens unless requested.

### Feature-flag safety

- A disabled feature must be hidden or disabled consistently across navigation, commands and relevant entry points.
- A feature flag must not be treated as an authorisation or security boundary.
- Remote flags must never bypass authentication or server-side permissions.
- Do not make a flag depend on hardcoded UI state.
- Do not scatter literal feature-key strings throughout the code.
- Group keys in a central, typed registry or equivalent.
- Provide tests for default values, remote overrides, cached configuration and unavailable endpoints.

If a required core feature is disabled, provide a sensible fallback screen or route rather than crashing.

---

## 13. MOCK DATA AND FUTURE API INTEGRATION

The API is not ready. The application must nevertheless work end to end using mock services.

Create interfaces such as:

- `IAuthenticationService`
- `IHomeService`
- `IRechargeService`
- `IRechargePlanService`
- `ITransactionService`
- `IFavouritesService`
- `IProfileService`
- `ISupportService`
- `IFeatureFlagService`
- `IThemeService`
- `ILocalizationService`

Use consistent model and DTO contracts.

Create mock implementations, for example:

- `MockAuthenticationService`
- `MockRechargeService`
- `MockRechargePlanService`
- `MockTransactionService`
- `MockFavouritesService`
- `MockProfileService`
- `MockSupportService`
- `MockFeatureFlagService`

Register the mock implementations through dependency injection.

### Mock functionality

Provide representative data for:

- Mobile operators.
- DTH operators.
- Recharge plans.
- Different transaction statuses.
- Recent transactions.
- Saved mobile numbers.
- Saved DTH connections.
- Profile information.
- FAQs.
- Support issue categories.
- Feature-flag configuration.

Use deterministic mock data so unit tests and screenshots remain stable.

Make common user actions functional:

- Login using a documented development-only mock authentication flow.
- Verify an OTP through the mock service.
- Navigate to Home after successful mock verification.
- Select an operator and plan.
- Submit a mock recharge request.
- Display the service-returned simulated result.
- View the corresponding transaction.
- Search and filter history.
- Add, edit and delete favourites.
- Prefill a recharge form from a favourite.
- Change language.
- Change theme.
- Open Help Centre and support forms.

Do not merely make every button navigate to the same generic page.

### API-ready architecture

Prepare the application so that a future integration can replace mock services with real API services through dependency injection.

The UI and ViewModels should depend on interfaces, not concrete mock classes.

Prepare:

- API request and response DTOs.
- JSON serialization conventions.
- Configurable base URL.
- Typed HTTP clients where appropriate.
- Authentication handler abstraction.
- Timeout and cancellation handling.
- Error mapping.
- Retry policies only for operations that are safe to retry.
- Logging that excludes sensitive data.
- Mock and API environment selection.

Do not create an arbitrary production API URL. Use configuration placeholders and document the future setup.

Do not automatically retry a recharge submission unless the API supports a safe idempotency mechanism.

Do not store access tokens, OTPs, passwords or sensitive information in ordinary preferences or logs.

---

## 14. LOADING, SKELETONS AND RESPONSIVENESS

Use polished loading feedback.

### Loading states

Use:

- Skeleton cards while lists are loading.
- Skeleton plan cards while recharge plans load.
- Button-level loading indicators during submissions.
- Inline loading for small operations.
- Full-screen loading only when the whole screen genuinely cannot be used.
- Empty states when no data exists.
- Recoverable error states with retry actions.

Requirements:

- Avoid excessive spinners.
- Prevent duplicate submissions.
- Use cancellation tokens where appropriate.
- Handle slow or failed mock/API operations gracefully.
- Stop animations and loading indicators when work finishes or the page is disposed.
- Do not leave a button permanently disabled after an exception.

Use subtle animations only when they improve feedback. Respect reduced-motion preferences where practical.

### Performance

- Use compiled bindings (`x:DataType`) where supported.
- Avoid unnecessary UI-thread work.
- Avoid blocking calls such as `.Result` and `.Wait()` in asynchronous flows.
- Use async/await consistently.
- Use collection virtualisation or appropriate list controls for large collections.
- Avoid repeatedly decoding large images.
- Use image caching appropriately.
- Avoid unnecessary property-change notifications.
- Dispose resources correctly.
- Avoid memory leaks caused by event subscriptions or long-lived references.
- Keep the main thread responsive during data loading and filtering.

The app must feel responsive on normal Android devices, iPhones and desktop systems.

---

## 15. ERROR HANDLING AND LOGGING

Implement centralised, user-friendly error handling.

Handle:

- Invalid user input.
- Authentication failures.
- OTP expiry or mismatch.
- Network errors.
- Request timeout.
- API unavailable.
- Empty data.
- Mock service failures.
- Invalid configuration.
- Local persistence errors.
- Unexpected exceptions.

Requirements:

- Show understandable messages to users.
- Keep technical details in appropriate development logs.
- Never display stack traces to end users.
- Provide retry actions where appropriate.
- Distinguish validation errors from service failures.
- Preserve user-entered data when safe.
- Use cancellation tokens for navigation-sensitive requests.
- Avoid swallowing exceptions silently.
- Never log passwords, OTPs, authentication tokens or sensitive user data.
- Include correlation/reference identifiers where appropriate and available.

Document the central error-handling approach and how it will work when real APIs are introduced.

---

## 16. SECURITY AND PRIVACY

Although the initial implementation uses mock data, prepare the app responsibly for production.

- Do not embed production secrets.
- Do not hardcode API credentials.
- Use secure storage for sensitive session material when real authentication is integrated.
- Keep authentication and session logic behind service interfaces.
- Clear session state on logout.
- Validate all user inputs.
- Treat remote configuration as untrusted input.
- Do not rely on feature flags for access control.
- Do not represent mock transactions as actual financial transactions.
- Do not collect unnecessary personal information.
- Avoid logging complete mobile numbers when masking is sufficient.
- Document security tasks that must be completed before production release.

---

## 17. CROSS-PLATFORM RESPONSIVENESS

The design should closely match the supplied mobile mockups while adapting appropriately to desktop.

### Android and iOS

- Respect safe areas and system bars.
- Handle portrait and supported landscape layouts.
- Support keyboard appearance and dismissal.
- Prevent input fields and primary actions from being hidden behind the keyboard.
- Use platform-appropriate back navigation.
- Verify touch targets and scroll behaviour.
- Verify image rendering and fonts.
- Test app lifecycle transitions.

### Windows and Mac Catalyst

- Support resizable windows.
- Set sensible minimum window dimensions where supported.
- Avoid stretching mobile layouts awkwardly across a wide screen.
- Use appropriate content width constraints and responsive layouts.
- Keep navigation and forms accessible with mouse, keyboard and touch where available.
- Ensure focus order and keyboard interactions work.
- Reuse the same design system while adapting layout for desktop.

Do not create unrelated designs for each platform. The brand, components, navigation semantics and visual hierarchy must remain consistent.

---

## 18. UNIT TESTING AND QUALITY ASSURANCE

Create automated tests with xUnit and suitable mocking tools only when needed.

Test at least:

### Authentication

- Valid mobile number.
- Invalid mobile number.
- Mock OTP success.
- Invalid OTP.
- Expired OTP.
- Duplicate submission handling.
- Logout/session reset.

### Recharge

- Mobile number validation.
- DTH subscriber ID validation.
- Operator selection.
- Plan selection.
- Invalid amount.
- Mock success, failure and pending results.
- Cancellation and service exceptions.
- No accidental duplicate submission.

### History

- Search by supported fields.
- Filter by service type.
- Filter by status.
- Filter by date range.
- Clear filters.
- Empty result handling.

### Favourites

- Add mobile favourite.
- Add DTH favourite.
- Edit favourite.
- Delete favourite.
- Duplicate handling.
- Default amount validation.
- Prefill correct recharge type and operator.

### Configuration

- Feature-flag defaults are `true`.
- Remote configuration overrides defaults.
- Fallback works when configuration cannot be fetched.
- Theme preference persists.
- Language preference persists.

### General quality

- ViewModel commands.
- Error-state handling.
- Service registration.
- Model serialization.
- Navigation decisions that can be tested independently.

Use deterministic mock data. Avoid tests that depend on live APIs, live recharges or external credentials.

Where feasible, add UI automation or smoke tests for login, Home, Mobile Recharge, DTH Recharge and favourites.

---

## 19. DEVELOPMENT COMMENTS AND DOCUMENTATION

Use meaningful comments throughout the code where they clarify intent, architecture, platform-specific behaviour or non-obvious business rules.

Do not add comments that merely repeat the code.

Include documentation for:

- Solution architecture.
- Folder responsibilities.
- Navigation flow.
- Feature flags.
- Mock service behaviour.
- How to replace mocks with real API implementations.
- Theme and language configuration.
- Adding a new screen.
- Adding a new reusable component.
- Running tests.
- Building for each supported platform.
- Required platform SDKs and signing configuration.
- Known limitations and remaining production tasks.

Add a `README.md` with setup and execution instructions.

Maintain a `IMPLEMENTATION_STATUS.md` checklist tracking every screen, test, platform build and unresolved issue.

---

## 20. STRICT UI MATCHING RULES

The application must closely match the supplied mockups.

For each screen:

1. Match the overall layout and content hierarchy.
2. Match the header, form and card proportions.
3. Match colours, gradients, typography and spacing.
4. Match the visual style of buttons and text fields.
5. Match icons and imagery using the provided assets.
6. Match the bottom navigation design.
7. Match loading, empty and error states to the same design system.
8. Verify the actual rendered screen against the mockup.

Do not:

- Replace the design with a generic template.
- Introduce random gradients or new colours.
- Change the bottom tab bar on individual screens.
- Add features that are outside the current product scope.
- Add Scan & Pay or money-transfer actions.
- Add wallet funding or Add Money.
- Add distributor, retailer, dealer, commission or My Users features.
- Add notification or payment-confirmation screens.
- Use invented operator logos as if they were official.
- Remove existing functionality simply to simplify the UI.
- claim pixel-perfect matching without comparing rendered results.

If a mockup conflicts with platform conventions, adapt it minimally and document the reason.

---

## 21. DEVELOPMENT PHASES

Implement in small, reviewable phases. Do not generate the entire application as one unverified block of code.

### Phase 1 — Inspect and prepare

- Inspect the repository, SDK version, project files and available platforms.
- Locate the design reference package.
- Confirm the logo and splash assets.
- Review the supplied design tokens and screen mockups.
- Establish a baseline build.
- Document the implementation plan and any missing assets.

### Phase 2 — App foundation

- Configure app icon and splash screen.
- Establish project architecture.
- Register dependency injection.
- Configure resources, styles and fonts.
- Set up navigation.
- Implement central error handling.
- Implement feature flags.
- Configure themes and localisation.
- Add initial unit tests.

### Phase 3 — Authentication

- Implement Login.
- Implement OTP Verification.
- Add mock authentication.
- Test validation, loading, errors and navigation.

### Phase 4 — Core recharge experience

- Implement Home.
- Implement Mobile Recharge.
- Implement DTH Recharge.
- Implement mock plans and recharge operations.
- Implement transaction details.

### Phase 5 — History and favourites

- Implement Recharge History and filters.
- Implement Favourites.
- Implement all Add Favourite steps.
- Test search, filtering, editing, deleting and prefilled recharge.

### Phase 6 — Profile and support

- Implement Profile & Settings.
- Implement Help & Support.
- Implement Help Centre.
- Implement Contact Support and Report an Issue.

### Phase 7 — Cross-platform polish

- Verify Android, iOS, Windows and Mac Catalyst compatibility.
- Check responsive layouts, safe areas and keyboard behaviour.
- Verify theme and language switching.
- Check loading and error states.
- Profile performance and correct identified issues.

### Phase 8 — Final verification

- Build every platform that can be built in the current environment.
- Run unit tests.
- Fix errors introduced by the implementation.
- Verify navigation and functional flows.
- Compare rendered screens against the mockups.
- Update documentation and implementation status.

If a platform cannot be built because the current machine lacks the required SDK, simulator, signing setup or operating system, report the exact blocker. Do not claim that platform was verified.

---

## 22. DEFINITION OF DONE

The application is not complete until:

- The solution has a maintainable architecture.
- App icon and splash assets are configured.
- Login and OTP flows work with mock services.
- Home displays the approved B2C features.
- Mobile and DTH Recharge are visually consistent.
- Recharge results and transaction history work with mock data.
- Favourites can be added, edited, deleted and used to prefill recharge.
- Profile and support screens are navigable.
- Theme and language preferences work and persist.
- Feature flags have defaults of `true` and support remote overrides through an abstraction.
- Reusable controls and centralised styling are in place.
- Loading, skeleton, empty and error states are implemented.
- Unit tests cover core business logic.
- API integration points are documented.
- The rendered UI has been compared with the supplied mockups.
- Supported builds and test results are recorded honestly.
- No out-of-scope B2B or unsupported payment features have been introduced.

---

## 23. FINAL INSTRUCTION TO THE CODING AGENT

Begin by inspecting the current repository and locating `ThogaiPay_MockUI_and_Assets`.

Do not modify unrelated files or remove working code without a clear reason.

Start with the application icon and full-screen splash configuration using `thogai_logo` and `thogai_splash_screen`. Then establish the architecture and implement the application in the phases above.

Use mock data for all backend-dependent features. Keep the service interfaces and DTOs ready for real API integration.

**The design reference folder is mandatory. Inspect the matching screen's `MockUI` and `Assets` folders before implementing each screen, move the required assets into MAUI resources, and closely reproduce the mockup using reusable native controls.**

Work incrementally. Build, test, inspect the actual result and fix issues before proceeding.

At the end of each phase, report:
1. What was implemented.
2. Which files were created or changed.
3. Which functionality is working.
4. Which tests and builds passed or failed.
5. Which screens were compared against their mockups.
6. Any missing assets, platform limitations or remaining work.

Do not claim completion for unimplemented features or unverified platforms.