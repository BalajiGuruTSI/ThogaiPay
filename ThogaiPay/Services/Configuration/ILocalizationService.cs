namespace ThogaiPay.Services.Configuration;

/// <summary>
/// Interface for managing application language and localization.
/// </summary>
public interface ILocalizationService
{
    /// <summary>
    /// Gets or sets the current language.
    /// </summary>
    string CurrentLanguage { get; set; }

    /// <summary>
    /// Initializes the localization service and applies saved preference.
    /// </summary>
    /// <returns>Completion task.</returns>
    Task InitializeAsync();

    /// <summary>
    /// Gets a localized string for the given key.
    /// </summary>
    /// <param name="key">Resource key (language-specific).</param>
    /// <returns>Localized string or the key itself if not found.</returns>
    string GetString(string key);

    /// <summary>
    /// Event raised when language changes.
    /// </summary>
    event EventHandler<string>? LanguageChanged;

    /// <summary>
    /// Supported language codes.
    /// </summary>
    string[] SupportedLanguages { get; }
}
