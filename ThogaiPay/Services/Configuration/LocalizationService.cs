using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ThogaiPay.Services.Configuration;

public class LocalizationService : ILocalizationService
{
    private string _currentLanguage = "en";

    // Minimal in-memory resource map for demonstration; expand for real app.
    private readonly Dictionary<string, Dictionary<string, string>> _resources = new()
    {
        ["en"] = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["WelcomeBack"] = "Welcome Back! 👋",
            ["LoginSubtitle"] = "Login to your ThogaiPay account\nto continue",
            ["SwipeToSendOtp"] = "Swipe to Send OTP",
            ["VerifyOtpTitle"] = "Verify OTP",
            ["OtpSentTo"] = "We have sent a 6-digit OTP to",
            ["SwipeToResend"] = "Swipe to Resend",
            ["SwipeToVerifyOtp"] = "Swipe to Verify OTP",
            ["EditLabel"] = "✎",
        },
        ["ta"] = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["WelcomeBack"] = "வாய்னுக்கு மீண்டும் வருக! 👋",
            ["LoginSubtitle"] = "தொடர உள்ள உங்கள் ThogaiPay கணக்கில் கிளிக் செய்து நுழையவும்",
            ["SwipeToSendOtp"] = "OTP அனுப்ப ஸ்வைப் செய்யவும்",
            ["VerifyOtpTitle"] = "OTP சரிபார்ப்பு",
            ["OtpSentTo"] = "6-அடி OTP அனுப்பப்பட்டுள்ளது",
            ["SwipeToResend"] = "மறுபடியும் அனுப்ப ஸ்வைப் செய்யவும்",
            ["SwipeToVerifyOtp"] = "OTP ஐ சரிபார்க்க ஸ்வைப் செய்யவும்",
            ["EditLabel"] = "✎",
        }
    };

    public string CurrentLanguage
    {
        get => _currentLanguage;
        set
        {
            if (_currentLanguage == value) return;
            _currentLanguage = value;
            LanguageChanged?.Invoke(this, _currentLanguage);
        }
    }

    public event EventHandler<string>? LanguageChanged;

    public string[] SupportedLanguages => new[] { "en", "ta" };

    public Task InitializeAsync()
    {
        // Minimal: use default language.
        _currentLanguage = "en";
        return Task.CompletedTask;
    }

    public string GetString(string key)
    {
        if (string.IsNullOrWhiteSpace(key)) return string.Empty;
        if (_resources.TryGetValue(_currentLanguage, out var map) && map.TryGetValue(key, out var value))
        {
            return value;
        }

        // Fallback to english
        if (_resources.TryGetValue("en", out var enMap) && enMap.TryGetValue(key, out var enValue))
        {
            return enValue;
        }

        return key; // fallback to key
    }
}
