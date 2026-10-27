# THOGAIPAY — GREENFIELD ARCHITECTURE, DESIGN SYSTEM & WHITE-LABEL PLATFORM
## Master Implementation Prompt v2 — Brand-Driven .NET MAUI Foundation

## 1. Mission

Act as a Principal Software Architect, Senior .NET MAUI Engineer, Fintech Reliability Engineer, Design System Architect, and Code Quality Engineer. Create a maintainable, secure-by-design, testable, reusable, scalable, commercial white-label foundation for ThogaiPay.

This is a greenfield replacement task. Inspect the existing repository and approved mockups only for business requirements and reusable assets. Preserve the original project and user files. Work in a new destination directory. If the destination contains an unrelated project, stop rather than deleting or overwriting it. Do not deliver only a plan, folders, interfaces, or empty skeletons; implement meaningful vertical slices, validation, tests, and documentation.

Do not claim to reproduce any company's private architecture. Apply established engineering practices appropriate to a fintech mobile client.

## 2. Core requirement: brand identity drives the build

The selected and validated brand manifest is the **single source of truth** for customer-specific identity and branding. The package/application identifier is one output of the manifest; do not try to infer every design setting from an arbitrary package string or duplicate identity settings across the codebase.

Each brand lives at `branding/brands/<brandId>/brand.json` and includes:

- `brandId`, `displayName`, `appId`, `urlScheme`;
- canonical brand colors and supported semantic palette overrides;
- app icon, splash, logo, and in-app asset references;
- default locale and supported locales;
- permitted client-side feature flags;
- only non-secret public configuration.

Never put signing keys, certificates, payment credentials, API secrets, or private tokens in a brand manifest.

### Required identity/resource mapping

- `displayName` → native application display title and app-visible name where appropriate.
- `appId` → native application/package identifier.
- `urlScheme` → generated build metadata and platform-specific URL registration; do not claim deep-link support until each platform is wired and tested.
- `colors` → generated semantic design tokens consumed by shared styles.
- `assets.appIcon` → native application icon.
- `assets.splash` → native splash image and background color.
- `localization` → supported/default locale configuration validated against actual resources.
- `features` → client feature configuration only; never use client flags as authorization or a security boundary.

### Brand selection and build workflow

Provide a single explicit build interface that accepts a `BrandId`. It must:

1. Load exactly one manifest.
2. Validate schema, required values, application identifier, URL scheme, palette, locales, feature flag types, and asset paths/formats.
3. Reject path traversal and missing assets.
4. Generate deterministic outputs only under designated generated directories.
5. Generate build properties and semantic resource dictionaries before MSBuild evaluates the application project.
6. Ensure the generated `ApplicationTitle`, `ApplicationId`, selected icon, splash, and semantic colors all come from the same manifest.
7. Fail on invalid manifests or identity mismatches; do not silently fall back to another brand.
8. Offer validation-only and dry-run modes where practical.
9. Report selected brand, generated files, and errors clearly.
10. Never overwrite unrelated files or delete user/customer assets automatically.

Document the actual executable commands. A supported simple interface should be equivalent to:

```bash
python tools/branding/brand.py validate --brand thogaipay
python tools/branding/brand.py generate --brand thogaipay
python tools/build/build_brand.py --brand thogaipay --framework net10.0-android
```

Use actual implemented syntax, not hypothetical commands. When switching brands, generate the selected brand before building. Include a second sample brand with a different display name, application identifier, URL scheme, palette, assets, and locale settings. Prove that changing brands does not require editing individual feature screens.

## 3. Approved ThogaiPay brand standards

The initial ThogaiPay brand must use these exact canonical values in one source of truth:

- Navy `#101F50`
- Sunrise orange `#F59E0B`
- Green `#16A34A`
- White `#FFFFFF`
- Blue accent `#2563EB`
- Cyan accent `#06B6D4`

Use navy for primary brand identity and principal text; orange as a deliberate accent; green primarily for positive states; blue for links, focus, and interaction; cyan sparingly; and white/neutral surfaces for clarity. Do not use all colors indiscriminately. Define and test accessible foreground/background combinations.

Create coherent light and dark themes. Generate or select brand-specific semantic resource mappings, with dark mappings that preserve identity while maintaining readable contrast. Runtime theme changes may switch theme resources; native app identity, package ID, icon, and splash are build-time properties.

## 4. Technology and repository discovery

Use .NET MAUI, C#, XAML, CommunityToolkit.Mvvm, Microsoft.Extensions.DependencyInjection, System.Text.Json, typed HttpClient, SQLite with explicit SQL, Microsoft.Extensions.Logging, and xUnit or a justified maintained test framework. Do not use Entity Framework Core.

Before choosing target frameworks or packages, inspect the installed .NET SDKs, MAUI workloads, platform tooling, and package compatibility. Pin the SDK with `global.json`, centralize package versions in `Directory.Packages.props`, and use consistent compiler settings. Build only supported targets and report unavailable workloads honestly. Do not claim compilation or tests passed unless they were executed.

## 5. Solution architecture and dependency direction

Recommended projects:

- `ThogaiPay.App`: MAUI entry point, Views, ViewModels, Shell/navigation, reusable UI controls, design system, localization, brand resource loading, and composition root.
- `ThogaiPay.Domain`: entities, value objects, enums, domain exceptions, rules, and pure calculations. No MAUI, Application, Infrastructure, SQLite, or HTTP dependencies.
- `ThogaiPay.Application`: use cases, application-facing service/repository interfaces, contracts, validation, result/error abstractions, and workflow orchestration. Depends on Domain only.
- `ThogaiPay.Infrastructure`: API clients, mock implementations, SQLite repositories, serialization, mapping, diagnostics, and technical adapters. Implements Application contracts; must not reference App.
- Tests: Domain, Application, Infrastructure, and Architecture tests.

Dependency direction: App → Application; App → Infrastructure only for composition; Application → Domain; Infrastructure → Application and Domain; Domain → no project dependencies. Enforce boundaries through automated tests. Do not use a global service locator, unnecessary generic repositories, circular dependencies, or architecture patterns without demonstrated need.

Organize presentation code feature-first: Authentication, Home, Recharge, Payments, History, Favourites, Account/Profile, Notifications, Offers, and Help. Put shared components in a shared area. Create only folders and abstractions with real responsibilities.

## 6. Design system and shared controls

Keep semantic tokens separate from raw brand values. Include at least:

- BrandPrimary, BrandSecondary, BrandAccent, BrandSuccess;
- SurfaceDefault, SurfaceElevated, SurfaceSecondary, BorderDefault, DividerDefault;
- TextPrimary, TextSecondary, TextSubtitle, TextDisabled, TextPlaceholder, TextLabel;
- primary/secondary action background and foreground states;
- input background, border, focused border, error border, floating label, and placeholder;
- success, warning, error, and information states;
- navigation background, selected/unselected states, and indicator.

Centralize typography, spacing, radii, elevations, dimensions, and motion. Use DynamicResource where runtime theme/resource changes require it; use StaticResource for fixed values. Keep resources offline and local.

Implement and document reusable controls where they have actual usage: PrimaryButton, SecondaryButton, FloatingLabelEntry, QuickRechargeSearchBar, AppHeader, SectionHeader, StandardCard, FavouriteRechargeCard, TransactionCard, OfferCard, StatusBadge, LoadingView, EmptyStateView, and ErrorStateView. Include disabled/focus/error states, touch targets, accessibility semantics, and a component catalog. Detect duplicate resource keys and unauthorized inline styling where feasible. Feature screens must not hardcode brand colors or customer assets.

## 7. Localization

Use actual `.resx` resources and strongly typed accessors, with English and Tamil initially. Centralize titles, buttons, placeholders, validation messages, navigation labels, accessibility descriptions, status messages, and empty/error states. Support runtime language switching and notify open UI elements. Use culture-aware date, number, and currency formatting. Add a validation tool for missing keys/translations. Validate configured supported locales against real resources; metadata alone is not localization implementation.

## 8. MVVM, navigation, and screens

Use CommunityToolkit.Mvvm appropriately: ObservableObject, ObservableProperty, RelayCommand, and AsyncRelayCommand. BaseViewModel should contain only genuinely shared busy/error/info/validation and cancellation-aware operation behavior. Use `try/finally` for busy-state cleanup, prevent duplicate commands where appropriate, and avoid `async void` except framework-required handlers. No HTTP, database, payment rules, or feature business logic in code-behind or ViewModels.

Use compiled XAML bindings with `x:DataType` wherever feasible. Register pages and ViewModels through DI. Centralize Shell routes, titles, icons, navigation parameters, back behavior, and authentication-gated routes. Main navigation must support Home, Recharge, central primary action where required by approved mockups, History, and Account. Do not create competing Shell instances.

Implement meaningful initial flows for Authentication, Home, Mobile/DTH Recharge, History, and Account. Use approved mockups/assets when available. Include loading, empty, error, offline, and success/pending states.

## 9. Contracts, mock data, and API-ready infrastructure

Separate Domain entities, Application contracts, API DTOs, persistence models, and UI models. Use exact monetary representations (`decimal` or equivalent) with explicit currency and rounding rules. Persist event timestamps in UTC and localize them for display.

Implement application-facing interfaces only for actual required capabilities, such as recharge, transactions, favourites, profile, and payments. Mock and API providers must implement the same interfaces. Select `Mock` or `Api` centrally at startup; validate selection. Use deterministic mock fixtures for success, empty collections, validation errors, timeout, server errors, pending, and failure. Label mock mode visibly and never represent mock activity as genuine money movement. Never silently replace API failure with mock success.

Implement typed HttpClient registration, configurable base URLs/timeouts, System.Text.Json settings, cancellation, consistent error translation, response validation, structured/redacted logs, correlation IDs where appropriate, and safe retries only for operations that are safe to retry. Mark provisional API contracts as pending backend agreement. Do not invent provider endpoints or credentials.

## 10. Financial reliability

Model transaction states and valid transitions explicitly, including Initiated, Pending, Successful, Failed, and Cancelled where appropriate. Distinguish a recharge request, payment attempt, and server-confirmed outcome. Prevent duplicate submissions and define server-supported idempotency contracts, status polling, pending recovery, and reconciliation extension points.

A timeout, local database write, or submitted request does not prove success or failure. Do not automatically retry payment creation unless an approved idempotency strategy makes it safe. Treat the backend/approved provider as authoritative for real outcomes. Do not store OTPs, UPI PINs, card details, banking passwords, or payment credentials in ordinary storage or logs. Mock payments must remain clearly simulated.

## 11. SQLite and persistence

Use a maintained SQLite library compatible with the selected target. Implement database initialization, version tracking, ordered migrations, parameterized SQL, indexes where justified, transaction boundaries, repository implementations, and migration tests. Do not run destructive migrations or database resets during normal production startup. Document the source of truth for cached, remote, and locally created records. Do not build a general-purpose offline sync engine until a real feature requires it.

## 12. Security, diagnostics, and environments

Support Development, Staging, and Production with separate brand identity, runtime settings, endpoints, provider selection, and logging. Validate configuration at startup. Release builds must not accidentally enable development mock behavior. Use HTTPS, secure platform storage for appropriate tokens, redacted structured logs, safe error messages, cancellation, and package vulnerability checks. Never disable certificate validation in production. Document signing and platform security requirements without committing signing material. Do not claim regulatory compliance or security certification without evidence.

## 13. Quality gates and documentation

Tests must cover domain validation, money calculations/rounding, transaction transitions, duplicate submission, mock behavior, API mapping, repository behavior, SQLite initialization/migrations, configuration validation, brand generation, localization completeness, and architecture boundaries where applicable.

Provide scripts for restore/build/test, brand validation/generation, asset validation, localization validation, duplicate resource detection, inline-color checks, mock fixture validation, and architecture checks. Add CI where supported. Consider dependency vulnerability checks, license inventory, and SBOM generation.

Documentation must match the implementation and cover architecture/dependency rules/data flow/transaction lifecycle, local setup, adding screens/services, mock-to-API switching, database migrations, white-label workflow, design system/component catalog, localization, security checklist, build/release, dependency inventory, and release checklist.

## 14. Implementation sequence

1. Inspect SDKs/workloads, repository, approved assets, and destination safety.
2. Preserve the original project and create a clean solution.
3. Establish project references and architecture tests.
4. Implement domain rules and application contracts/use cases.
5. Implement brand manifests, schema validation, generator, and build wrapper.
6. Implement semantic tokens, light/dark mappings, shared styles, and controls.
7. Implement English/Tamil localization and validation.
8. Implement MVVM base behavior, centralized navigation, and functional feature slices.
9. Implement deterministic mocks, provider selection, typed API clients, and SQLite persistence.
10. Implement transaction reliability rules and tests.
11. Add CI, quality gates, documentation, and release tooling.
12. Validate both sample brands and verify generated identity/resource consistency.
13. Restore, build supported targets, run tests, fix issues, and report actual results.

## 15. Mandatory brand acceptance tests

- Each BrandId resolves to exactly one valid manifest.
- ThogaiPay's six canonical colors are exact and centralized.
- Selected `ApplicationTitle` and `ApplicationId` match the manifest.
- Generated semantic resource values match the selected manifest.
- Selected icon and splash come from that manifest.
- Both brands generate different intended identities/palettes without screen edits.
- Invalid configuration and identity mismatch fail safely.
- Repeated generation is deterministic.
- Light/dark theme mappings retain the selected brand identity and readable contrast.
- All referenced static resource keys exist.
- Commands documented in README are implemented and executed where the environment permits.

## 16. Final report

Report the directory tree and project responsibilities, implemented features, brand workflow, mock-to-API workflow, exact build/test commands and actual results, supported/unsupported platforms, known gaps, and unresolved backend/platform requirements. Clearly distinguish implemented, compiled, tested, device-verified, and production-ready. Do not claim any result that was not actually verified.

The goal is not the largest architecture. It is a reliable foundation in which brand identity, design tokens, shared controls, application logic, and data providers evolve independently.
