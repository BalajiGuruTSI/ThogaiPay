using Microsoft.Maui.Storage;

namespace ThogaiPay.App.Shared.Localization;

/// <summary>Application-wide language preference and string lookup. Defaults to English.</summary>
public sealed class LocalizationService : ILocalizationService
{
    private const string PreferenceKey = "thogaipay.language";
    private string _currentLanguage = Preferences.Default.Get(PreferenceKey, "en");

    private static readonly IReadOnlyDictionary<string, string> English = new Dictionary<string, string>
    {
        ["WelcomeTitle"] = "Welcome Back",
        ["LoginSubtitle"] = "Login to your ThogaiPay account",
        ["MobileFieldLabel"] = "Mobile number",
        ["MobilePlaceholder"] = "Enter mobile number",
        ["ContinueText"] = "Continue  →",
        ["ContinueBusyText"] = "Checking...",
        ["OrText"] = "or",
        ["CreateAccountText"] = "♙+   Create New Account",
        ["TrustText"] = "Secure & Trusted",
        ["InvalidMobile"] = "Enter a valid 10-digit Indian mobile number.",
        ["ValidMobilePending"] = "Your number is valid. OTP delivery will be enabled when authentication is connected.",
        ["CreateAccountPending"] = "Account creation will be available when registration is connected."
    };

    private static readonly IReadOnlyDictionary<string, string> Tamil = new Dictionary<string, string>
    {
        ["WelcomeTitle"] = "மீண்டும் வரவேற்கிறோம்",
        ["LoginSubtitle"] = "உங்கள் ThogaiPay கணக்கில் உள்நுழையவும்",
        ["MobileFieldLabel"] = "மொபைல் எண்",
        ["MobilePlaceholder"] = "மொபைல் எண்ணை உள்ளிடவும்",
        ["ContinueText"] = "தொடரவும்  →",
        ["ContinueBusyText"] = "சரிபார்க்கிறது...",
        ["OrText"] = "அல்லது",
        ["CreateAccountText"] = "♙+   புதிய கணக்கை உருவாக்கவும்",
        ["TrustText"] = "பாதுகாப்பானது மற்றும் நம்பகமானது",
        ["InvalidMobile"] = "சரியான 10 இலக்க இந்திய மொபைல் எண்ணை உள்ளிடவும்.",
        ["ValidMobilePending"] = "மொபைல் எண் சரியானது. அங்கீகார வசதி இணைக்கப்பட்டதும் OTP அனுப்பப்படும்.",
        ["CreateAccountPending"] = "பதிவு வசதி இணைக்கப்பட்டதும் புதிய கணக்கை உருவாக்கலாம்."
    };

    public string CurrentLanguage => _currentLanguage;
    public event EventHandler? LanguageChanged;

    public void SetLanguage(string languageCode)
    {
        var normalized = languageCode.Equals("ta", StringComparison.OrdinalIgnoreCase) ? "ta" : "en";
        if (_currentLanguage == normalized) return;

        _currentLanguage = normalized;
        Preferences.Default.Set(PreferenceKey, normalized);
        LanguageChanged?.Invoke(this, EventArgs.Empty);
    }

    public string GetString(string key)
    {
        var dictionary = _currentLanguage == "ta" ? Tamil : English;
        if (dictionary.TryGetValue(key, out var value)) return value;
        return English.TryGetValue(key, out value) ? value : key;
    }
}
