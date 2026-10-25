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