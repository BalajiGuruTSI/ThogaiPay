## 4A. ARCHITECTURAL GOVERNANCE AND IMPLEMENTATION REFINEMENTS

Treat this section as a mandatory refinement of the architecture and acceptance criteria.

### A. Feature Ownership

1. Organize presentation code by feature, with each feature owning its relevant Views, ViewModels, and presentation models.
2. Keep shared UI components, application-wide navigation, localization, and design-system resources in dedicated shared locations.
3. Organize Application use cases and contracts by business capability.
4. Avoid creating projects, abstractions, interfaces, folders, or classes without a real responsibility.
5. Do not introduce microservices, event buses, CQRS, a mediator framework, or generic repository layers without a demonstrated requirement.
6. Keep the client architecture appropriate for a mobile application backed by a separately deployed ASP.NET Core API.

### B. Dependency and Package Governance

1. Pin the .NET SDK and centralize package versions.
2. Enable nullable reference types and consistent compiler settings.
3. Validate project references and architectural dependency direction through automated tests.
4. Keep the Domain framework-independent.
5. Keep Application independent of MAUI, Infrastructure, and concrete transport or persistence implementations.
6. Allow App to reference Infrastructure only where required for application composition and dependency registration.
7. Do not use a global service locator.
8. Select maintained dependencies compatible with the actual installed SDK and supported platforms.

### C. White-Label Build Pipeline

1. Maintain a schema-validated manifest for every customer brand.
2. Separate brand identity, semantic design tokens, environment configuration, feature configuration, and secrets.
3. Generate brand-specific resources into an explicitly designated generated directory.
4. Keep generated files separate from hand-maintained source files.
5. Validate configuration and assets before modifying generated output.
6. Support build-time application identifiers, display names, URL schemes, native icons, splash screens, and platform-specific identity settings.
7. Make generation deterministic and report every changed or generated file.
8. Never overwrite unrelated files or delete customer assets automatically.
9. Verify that two sample brands build without editing individual feature screens.
10. Include a dry-run or validation-only mode where practical.

### D. Design-System Governance

1. Keep the six approved ThogaiPay palette values canonical.
2. Define semantic tokens independently of raw brand colors.
3. Use separate light and dark resource mappings.
4. Centralize typography, spacing, radii, control dimensions, elevations, and interactive states.
5. Use DynamicResource only for resources that must respond to runtime changes.
6. Validate accessible contrast and disabled, focused, error, success, and warning states.
7. Detect duplicate resource keys and unauthorized inline colors.
8. Document justified exceptions for unique screen-specific designs.

### E. Financial Workflow Integrity

1. Separate recharge requests, payment attempts, and authoritative transaction outcomes.
2. Model valid transaction state transitions in the Domain or Application layer.
3. Prevent duplicate submissions using explicit request identifiers and server-supported idempotency.
4. Represent pending and unknown outcomes explicitly.
5. Never infer transaction success or failure solely from a network timeout, client exception, or local database write.
6. Provide safe status-query and reconciliation extension points.
7. Do not retry payment creation automatically unless an approved idempotency contract makes the operation safe.
8. Treat the backend and approved payment provider as authoritative for real financial outcomes.
9. Label mock transactions visibly and keep simulated operations separate from genuine financial activity.
10. Do not store payment credentials, OTPs, UPI PINs, or banking passwords in ordinary persistence or logs.

### F. Data and Synchronization Boundaries

1. Define ownership and source-of-truth rules for server data, local preferences, cached records, mock fixtures, and locally created records.
2. Use SQLite only for suitable local persistence.
3. Implement ordered, versioned migrations and test migration paths.
4. Use parameterized SQL and explicit transaction boundaries.
5. Never perform destructive database resets or migrations automatically in production.
6. Do not build a general-purpose offline synchronization engine unless an implemented feature requires it.

### G. Release and Quality Gates

1. Validate configuration, package identity, asset availability, localization completeness, and provider selection before release.
2. Prevent accidental activation of development mock behavior in production builds.
3. Keep secrets outside source control and client-side configuration.
4. Add CI checks for supported builds, automated tests, architecture boundaries, branding, localization, and dependency vulnerabilities where tooling permits.
5. Record actual build and test results.
6. Distinguish implemented, compiled, tested, device-verified, and production-ready functionality.
7. Report unsupported platforms, unavailable workloads, unresolved API agreements, and unverified release requirements.
8. Never mark a release ready merely because the source compiles.

### H. Implementation Discipline

Prefer the simplest maintainable design that satisfies the real requirement.

Before adding a new abstraction, package, project, or architectural pattern, explain the responsibility it serves and why an existing framework capability is insufficient.

Implement vertical slices that demonstrate real behavior rather than isolated skeleton classes. At minimum, demonstrate one end-to-end mock-backed recharge journey, one transaction-history journey, configuration-driven branding, runtime localization, and tested persistence.

Do not claim that mock-backed journeys constitute real payment integration or production readiness.
## 4B. BRAND IDENTITY MUST DRIVE THE BUILD — MANDATORY

The selected and validated brand manifest is the single source of truth for customer-specific identity and design resources. The package identifier is one output of this configuration; it must not be used as a heuristic to guess the rest of the brand.

### Required manifest fields

Each brand manifest must include:

- `brandId`
- `displayName`
- `appId`
- `urlScheme`
- `colors` containing the six canonical ThogaiPay colors for the ThogaiPay brand, or the partner's explicitly configured palette for another brand
- `assets.appIcon` and `assets.splash`
- `localization.defaultLocale` and `localization.supportedLocales`
- `features` containing explicitly permitted boolean feature flags

Never include signing keys, private certificates, payment credentials, API secrets, or other confidential values in a brand manifest.

### Required build flow

1. Select one explicit `BrandId`.
2. Load exactly one manifest from `branding/brands/<BrandId>/brand.json`.
3. Validate schema, required values, package identifier, URL scheme, palette values, locale consistency, feature flag types, and asset paths.
4. Refuse paths outside the repository and fail if any required asset is missing.
5. Generate deterministic output only under `src/ThogaiPay.App/Branding/Generated/`.
6. Generate `Brand.Generated.props` for native build identity and selected icon/splash assets.
7. Generate `BrandResources.xaml` for semantic design tokens and compatibility resources consumed by shared styles.
8. Import the generated MSBuild properties so `ApplicationTitle`, `ApplicationId`, URL scheme metadata, and selected native resources match the manifest.
9. Build and test the selected brand. Record the selected manifest and actual result.

The generator must not delete unrelated files, modify manually maintained files outside its declared output set, or silently accept identity mismatches. A validation-only or dry-run mode must be available.

### Identity/resource mapping

- `displayName` → native application display title.
- `appId` → native package/application identifier.
- `urlScheme` → platform URL scheme configuration or a documented generated property consumed by platform-specific configuration.
- `colors` → generated semantic tokens such as `BrandPrimary`, `BrandSecondary`, `BrandAccent`, `BrandSuccess`, `SurfaceDefault`, `TextPrimary`, `ActionPrimaryBackground`, and `NavigationSelected`.
- `assets.appIcon` → selected native application icon resource.
- `assets.splash` → selected native splash-screen resource and configured splash background.
- `localization` → supported/default locale configuration validated against the actual resource files.
- `features` → permitted client feature configuration; never use client flags as a security authorization boundary.

Every feature screen and reusable control must consume semantic resources. Feature screens must not contain customer-specific colors, names, logos, or asset paths.

The selected brand determines the available resource palette. Runtime light/dark theme switching may change theme mappings but must not change the installed application's native identity. Native package identifiers, icons, and splash configuration are build-time settings.

### Two-brand proof

The repository must include a ThogaiPay brand and a separate sample partner brand with different display names, application identifiers, URL schemes, palettes, icons/splash assets, and supported locales. The partner brand must be generated and built without editing individual feature screens.

### Commands and acceptance

The implementation must provide real, documented commands equivalent to:

```bash
python tools/branding/brand.py validate --brand thogaipay
python tools/branding/brand.py generate --brand thogaipay
python tools/branding/brand.py validate --brand sample-partner
python tools/branding/brand.py generate --brand sample-partner
```

After generating the desired brand, build the MAUI project using the SDK/workload available in the environment. Do not document a build as successful unless it was executed.

Automated checks must verify manifest uniqueness, application identity mapping, canonical ThogaiPay colors, required asset existence, deterministic generation, and safe failure for invalid configurations. Report any platform-specific settings that are not yet wired to the generated values rather than claiming full support.

## 4C. CURRENT IMPLEMENTATION STATUS MUST BE AUDITED HONESTLY

A manifest, schema, folder, or documentation statement is not evidence that the behavior is implemented. Inspect the source code and verify the actual execution path from manifest selection through generated resources to native build output. Distinguish source implementation, validation-script results, successful compilation, automated test results, device verification, and production readiness.

## 4D. PACKAGE IDENTITY AND STYLE RESOURCE SYNCHRONIZATION

The updated consolidated prompt is `ThogaiPay_Master_Implementation_Prompt_v2.md`. Use that file as the current master specification; this addendum is supporting context, not a replacement for the complete prompt.

The selected brand manifest must generate both native identity and semantic styling from the same source. Do not parse the package name to guess colors at runtime. Provide a one-command build wrapper that validates and generates the selected brand before invoking MSBuild, because MSBuild imports are evaluated before build targets execute.

The shared XAML styles must reference generated semantic resources such as `BrandPrimary`, `BrandSecondary`, `TextPrimary`, `DarkBrandPrimary`, and `DarkSurfaceDefault`. Changing the selected brand must change the generated package ID, display name, icon/splash selection, and compiled semantic palette without feature-screen edits. The sample partner generation test must prove this and restore the default ThogaiPay generated state after the test.


## Additional build-error prevention: MAUI XAML resources and asset names

- Any `ResourceDictionary` loaded through `ResourceDictionary Source=...` must use a valid MAUI XAML compilation pattern. For class-backed dictionaries, keep `x:Class`, the matching partial C# class, namespace, filename, and `InitializeComponent()` synchronized.
- Validate that generated brand resource dictionaries and shared style dictionaries compile as MAUI XAML; do not leave them as uncompiled loose XAML files.
- Use MAUI-safe generated asset filenames containing lowercase letters, digits, and underscores only, starting and ending with a letter before the extension (for example `brandappicon.png` and `brandsplash.svg`). Do not generate hyphenated asset names.
- Add automated tests for resource dictionary class/namespace matching, generated asset naming, referenced resource keys, and generated file existence.
- On any XAML type-resolution error such as `Color` or `ResourceDictionary` not found, inspect the XAML Build Action, project target/workload context, `x:Class`, and code-behind pairing before changing package references.
