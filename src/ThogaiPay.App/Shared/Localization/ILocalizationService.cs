namespace ThogaiPay.App.Shared.Localization;

public interface ILocalizationService
{
    string CurrentLanguage { get; }
    event EventHandler? LanguageChanged;
    void SetLanguage(string languageCode);
    string GetString(string key);
}
