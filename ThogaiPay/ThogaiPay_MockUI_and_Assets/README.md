# ThogaiPay B2C Mock UI & Asset Reference

This package contains editable SVG mockups and shared SVG assets for the key B2C flows. Each screen has `MockUI/` and `Assets/` folders.

## Screens

- `01_LoginPage/` — **LoginPage**: Login screen with Tamil/English switch, logo, +91 mobile input, Send OTP, optional Google sign-in and security reassurance.
- `02_OTPVerification/` — **OTPVerification**: Six-digit OTP, masked phone number, edit number, resend timer and Verify & Continue.
- `03_HomePage/` — **HomePage**: Tamil-inspired header, greeting, quick recharge Mobile/DTH selector, favourites and recent recharges; no wallet/Add Money.
- `04_MobileRecharge/` — **MobileRecharge**: Mobile number input, operator cards, plan categories, recharge plans and Recharge Now.
- `05_DTHRecharge/` — **DTHRecharge**: Subscriber ID input, DTH operator cards, plan categories, plan list and Recharge Now; same layout system as Mobile Recharge.
- `06_RechargeHistory/` — **RechargeHistory**: Modern search and filter sheet/chips, date range, Mobile/DTH, status filters and transaction list.
- `07_TransactionDetails/` — **TransactionDetails**: Recharge receipt details, status, reference ID, operator, number, amount, timestamp and support action.
- `08_Favourites/` — **Favourites**: Saved mobile/DTH connections, search, quick recharge action, edit/delete and Add New Favourite.
- `09_AddFavouriteType/` — **AddFavouriteType**: Choose Mobile Number or DTH Connection.
- `10_AddMobileFavourite/` — **AddMobileFavourite**: Mobile number, operator selection, optional quick amount and validation.
- `11_AddDTHFavourite/` — **AddDTHFavourite**: Subscriber ID, DTH operator selection and optional preferred amount.
- `12_FavouriteNameAndSave/` — **FavouriteNameAndSave**: Display name, optional avatar, optional default amount and Save Favourite.
- `13_FavouriteSaved/` — **FavouriteSaved**: Success state and saved favourite summary, then Recharge Now or Add Another.
- `14_ProfileSettings/` — **ProfileSettings**: Personal profile, payment methods only if supported, My Favourites, transactions, security, help, preferences and logout.
- `15_HelpSupport/` — **HelpSupport**: Help centre, FAQs, contact support, report an issue and feedback.
- `16_HelpCentre/` — **HelpCentre**: Search FAQs and grouped support topics.
- `17_ContactSupport/` — **ContactSupport**: Contact channels and support hours, based on implemented capabilities.
- `18_ReportIssue/` — **ReportIssue**: Issue category, transaction reference, description and optional attachment.

## How to use in your code project

1. Copy this package under your repository, e.g. `DesignReference/ThogaiPay_MockUI_and_Assets/`.
2. Add `00_Shared/DesignSystem/UI_IMPLEMENTATION_PROMPT.md` to your coding agent's project instructions or paste it into each UI task.
3. Tell the agent the exact screen folder to implement and require it to inspect that screen's mockup and asset manifest first.
4. SVG mockups are design references. Translate them into the project's actual UI framework; do not embed a whole mockup SVG as the live app UI.

## Scope decisions reflected

- B2C only; Mobile and DTH recharge.
- No Scan & Pay, money transfer, Add Money/wallet funding, distributor tools, notifications page, or payment confirmation page.
- Shared modern floating bottom navigation with central ThogaiPay brand mark.
- Mobile and DTH recharge layouts remain consistent.
