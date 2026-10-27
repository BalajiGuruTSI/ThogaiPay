# Application-wide localization

- English (`en`) is the default language.
- `LocalizationService` is registered as a singleton in `MauiProgram`.
- The selected language is persisted with MAUI `Preferences` under `thogaipay.language`.
- `LanguageSwitcher` raises a language selection event; `LoginPage` updates the singleton service.
- ViewModels should inject `ILocalizationService`, subscribe to `LanguageChanged`, and raise `PropertyChanged` for localized properties.
- Add translations to the service dictionaries (or migrate the same keys to `.resx` resources) and keep the English keys complete.
- Never store localized UI strings directly in XAML or duplicate language state in individual ViewModels.
