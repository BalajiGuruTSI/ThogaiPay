# ThogaiPay Screen Reference Index

This file maps each screen to its design references, asset manifests, implementation files, and verification status.

> Approved auth three-panel reference image is currently available from user-provided input, but a standalone repository file at `DesignReference/Approved/ThogaiPay_Auth_Approved_Reference.png` is not present.

## Shared References

- Design tokens JSON: `00_Shared/DesignSystem/design-tokens.json`
- Design tokens CSS: `00_Shared/DesignSystem/design-tokens.css`
- UI implementation prompt: `00_Shared/DesignSystem/UI_IMPLEMENTATION_PROMPT.md`
- Shared logos: `00_Shared/Logo/`
- Shared icons: `00_Shared/Icons/`
- Shared images: `00_Shared/Images/`

## Screen Mapping

| Screen | MockUI Path | Design Notes Path | Asset Manifest Path | App View | App ViewModel | Shared Assets Used | Visual Status | Functional Status | Notes / Gaps |
|---|---|---|---|---|---|---|---|---|---|
| Splash | (Approved image; no dedicated MockUI file found) | N/A | N/A | `Views/Startup/SplashPage.cs` | N/A | `Resources/Splash/thogai_splash_screen.png`, `Resources/AppIcon/thogai_logo.png` | Phase 1 corrected | Startup transition implemented | Missing standalone approved PNG in repo path |
| Login | `01_LoginPage/MockUI/LoginPage_MockUI.svg` | `01_LoginPage/MockUI/LoginPage_DesignNotes.md` | `01_LoginPage/Assets/asset-manifest.json` | `Views/Authentication/LoginPage.xaml` | `ViewModels/Authentication/LoginViewModel.cs` | mark, wordmark, globe, phone, shield, footer/header art | Phase 1 corrected | Validation + OTP request wired | Runtime screenshot verification pending |
| OTP Verification | `02_OTPVerification/MockUI/OTPVerification_MockUI.svg` | `02_OTPVerification/MockUI/OTPVerification_DesignNotes.md` | `02_OTPVerification/Assets/asset-manifest.json` | `Views/Authentication/OtpVerificationPage.xaml` | `ViewModels/Authentication/OtpVerificationViewModel.cs` | mark, back icon, phone/lock/shield | Phase 1 corrected | Verify + resend + countdown wired | Runtime keyboard-open screenshot verification pending |
| Home | `03_HomePage/MockUI/HomePage_MockUI.svg` | `03_HomePage/MockUI/HomePage_DesignNotes.md` | `03_HomePage/Assets/asset-manifest.json` | `MainPage.xaml` | `MainPage.xaml.cs` | home, mobile, dth, heart, history, profile, plus, phone, temple header, footer line-art, brand mark/wordmark | Phase 2 implemented (first pass) | Static quick-recharge/favourites/recent UI wired | Runtime screenshot verification pending; tab actions not yet fully routed |
| Mobile Recharge | `04_MobileRecharge/MockUI/MobileRecharge_MockUI.svg` | `04_MobileRecharge/MockUI/MobileRecharge_DesignNotes.md` | `04_MobileRecharge/Assets/asset-manifest.json` | `Views/Recharge/MobileRechargePage.xaml` | `Views/Recharge/MobileRechargePage.xaml.cs` | home, mobile, dth, phone, temple header | Phase 2 implemented (first pass) | Navigable from Home quick recharge CTA | Runtime screenshot verification pending; operator/plan interactions static in first pass |
| DTH Recharge | `05_DTHRecharge/MockUI/DTHRecharge_MockUI.svg` | `05_DTHRecharge/MockUI/DTHRecharge_DesignNotes.md` | `05_DTHRecharge/Assets/asset-manifest.json` | Pending | Pending | Pending | Pending | Pending | Not implemented yet |
| Recharge History | `06_RechargeHistory/MockUI/RechargeHistory_MockUI.svg` | `06_RechargeHistory/MockUI/RechargeHistory_DesignNotes.md` | `06_RechargeHistory/Assets/asset-manifest.json` | Pending | Pending | Pending | Pending | Pending | Not implemented yet |
| Transaction Details | `07_TransactionDetails/MockUI/TransactionDetails_MockUI.svg` | `07_TransactionDetails/MockUI/TransactionDetails_DesignNotes.md` | `07_TransactionDetails/Assets/asset-manifest.json` | Pending | Pending | Pending | Pending | Pending | Not implemented yet |
| Favourites | `08_Favourites/MockUI/Favourites_MockUI.svg` | `08_Favourites/MockUI/Favourites_DesignNotes.md` | `08_Favourites/Assets/asset-manifest.json` | Pending | Pending | Pending | Pending | Pending | Not implemented yet |
| Add Favourite Type | `09_AddFavouriteType/MockUI/AddFavouriteType_MockUI.svg` | `09_AddFavouriteType/MockUI/AddFavouriteType_DesignNotes.md` | `09_AddFavouriteType/Assets/asset-manifest.json` | Pending | Pending | Pending | Pending | Pending | Not implemented yet |
| Add Mobile Favourite | `10_AddMobileFavourite/MockUI/AddMobileFavourite_MockUI.svg` | `10_AddMobileFavourite/MockUI/AddMobileFavourite_DesignNotes.md` | `10_AddMobileFavourite/Assets/asset-manifest.json` | Pending | Pending | Pending | Pending | Pending | Not implemented yet |
| Add DTH Favourite | `11_AddDTHFavourite/MockUI/AddDTHFavourite_MockUI.svg` | `11_AddDTHFavourite/MockUI/AddDTHFavourite_DesignNotes.md` | `11_AddDTHFavourite/Assets/asset-manifest.json` | Pending | Pending | Pending | Pending | Pending | Not implemented yet |
| Favourite Name and Save | `12_FavouriteNameAndSave/MockUI/FavouriteNameAndSave_MockUI.svg` | `12_FavouriteNameAndSave/MockUI/FavouriteNameAndSave_DesignNotes.md` | `12_FavouriteNameAndSave/Assets/asset-manifest.json` | Pending | Pending | Pending | Pending | Pending | Not implemented yet |
| Favourite Saved | `13_FavouriteSaved/MockUI/FavouriteSaved_MockUI.svg` | `13_FavouriteSaved/MockUI/FavouriteSaved_DesignNotes.md` | `13_FavouriteSaved/Assets/asset-manifest.json` | Pending | Pending | Pending | Pending | Pending | Not implemented yet |
| Profile & Settings | `14_ProfileSettings/MockUI/ProfileSettings_MockUI.svg` | `14_ProfileSettings/MockUI/ProfileSettings_DesignNotes.md` | `14_ProfileSettings/Assets/asset-manifest.json` | Pending | Pending | Pending | Pending | Pending | Not implemented yet |
| Help & Support | `15_HelpSupport/MockUI/HelpSupport_MockUI.svg` | `15_HelpSupport/MockUI/HelpSupport_DesignNotes.md` | `15_HelpSupport/Assets/asset-manifest.json` | Pending | Pending | Pending | Pending | Pending | Not implemented yet |
| Help Centre | `16_HelpCentre/MockUI/HelpCentre_MockUI.svg` | `16_HelpCentre/MockUI/HelpCentre_DesignNotes.md` | `16_HelpCentre/Assets/asset-manifest.json` | Pending | Pending | Pending | Pending | Pending | Not implemented yet |
| Contact Support | `17_ContactSupport/MockUI/ContactSupport_MockUI.svg` | `17_ContactSupport/MockUI/ContactSupport_DesignNotes.md` | `17_ContactSupport/Assets/asset-manifest.json` | Pending | Pending | Pending | Pending | Pending | Not implemented yet |
| Report Issue | `18_ReportIssue/MockUI/ReportIssue_MockUI.svg` | `18_ReportIssue/MockUI/ReportIssue_DesignNotes.md` | `18_ReportIssue/Assets/asset-manifest.json` | Pending | Pending | Pending | Pending | Pending | Not implemented yet |

## Verification Legend

- **Phase 1 corrected**: Updated against approved auth reference and screen-specific mock assets.
- **Pending**: Not yet implemented/refined in Phase 2.
- **Runtime screenshot verification pending**: Requires emulator/device capture comparison.
