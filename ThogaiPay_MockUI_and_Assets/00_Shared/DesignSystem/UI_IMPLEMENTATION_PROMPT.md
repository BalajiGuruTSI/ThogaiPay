# ThogaiPay UI Implementation Prompt (Project Instruction)

## Mandatory design-reference rule
Never implement or redesign a screen from memory. First locate and inspect its approved reference, screen-specific `MockUI` and `Assets` folders, and `asset-manifest.json`. Use the approved reference as the visual source of truth. If a required reference or asset is missing, document the gap instead of inventing a replacement.

## Required workflow for every screen
1. Identify the target screen and locate its exact folder under `ThogaiPay_MockUI_and_Assets/`.
2. Read:
   - `MockUI/*_MockUI.svg`
   - `MockUI/*_DesignNotes.md`
   - `Assets/asset-manifest.json`
3. Inspect all referenced shared resources:
   - `00_Shared/DesignSystem/design-tokens.json`
   - `00_Shared/DesignSystem/design-tokens.css`
   - `00_Shared/Logo/`
   - `00_Shared/Icons/`
   - `00_Shared/Images/`
4. Apply reference precedence when conflicts exist:
   1) approved screen reference image,
   2) screen-specific mock,
   3) shared tokens/assets,
   4) existing app conventions,
   5) minimal platform adaptation.
5. Copy required assets into MAUI resources (`Resources/Images`, `Resources/AppIcon`, `Resources/Splash`, etc.) using valid MAUI-compatible filenames.
6. Implement using native MAUI controls and reusable components (do not embed mock SVG as runtime UI).
7. Preserve existing functionality, service interfaces, navigation contracts, and validation behavior.
8. Verify keyboard behavior, safe areas, responsive layout, loading/empty/error states.
9. Compare rendered UI with reference and correct mismatches before completion.
10. Build/test affected targets and report changed files + blockers honestly.
11. Update `SCREEN_REFERENCE_INDEX.md` with paths, assets used, and verification status.

## Scope guardrails
- B2C only.
- No Scan & Pay.
- No money transfer.
- No Add Money/wallet funding.
- No distributor/retailer/My Users tools.
- No notification centre.
- No separate payment-confirmation module.

Task rule: implement only the screen(s) requested, following these references without regressions.
