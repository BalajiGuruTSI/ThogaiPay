# ADR-004: Feature-Owned Screens and Shared Primary CTA

## Status
Accepted

## Context
The project structure agreement requires feature screens to live with their feature ViewModels, while shared controls live in a dedicated shared location. Repeated primary CTA styling also needs to respond to the selected white-label brand.

## Decision
- Feature pages are placed under `Features/<Feature>/Views` and their ViewModels under `Features/<Feature>/ViewModels`.
- App-wide startup UI is placed under `Shared/Startup`.
- The shared `PrimaryActionButton` lives under `Shared/Controls/Buttons`.
- Its semantic colors come from generated `DynamicResource` keys.
- Busy progress is shown inside the CTA surface, not in a separate loader.

## Consequences
- A feature screen cannot introduce customer-specific CTA colors by editing local XAML.
- The component owns busy visual state and disables repeat activation.
- A busy state is used only for real asynchronous work; local validation does not pretend to be an API operation.
