using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace ThogaiPay.App.Shared.Controls.Navigation;

/// <summary>
/// A compact, accessible segmented language switch. The selected language is
/// shared with the application's localization service by the hosting page.
/// </summary>
public partial class LanguageSwitcher : ContentView
{
    public static readonly BindableProperty SelectedLanguageProperty =
        BindableProperty.Create(
            nameof(SelectedLanguage),
            typeof(string),
            typeof(LanguageSwitcher),
            "en",
            BindingMode.TwoWay,
            propertyChanged: OnSelectedLanguageChanged);

    public event EventHandler<string>? LanguageSelected;

    public LanguageSwitcher()
    {
        InitializeComponent();
        UpdateSelection();
    }

    public string SelectedLanguage
    {
        get => (string)GetValue(SelectedLanguageProperty);
        set => SetValue(SelectedLanguageProperty, NormalizeLanguage(value));
    }

    private static void OnSelectedLanguageChanged(
        BindableObject bindable,
        object oldValue,
        object newValue)
    {
        ((LanguageSwitcher)bindable).UpdateSelection();
    }

    private void OnEnglishTapped(object? sender, TappedEventArgs e) => SelectLanguage("en");

    private void OnTamilTapped(object? sender, TappedEventArgs e) => SelectLanguage("ta");

    private void SelectLanguage(string languageCode)
    {
        if (SelectedLanguage == languageCode)
            return;

        SelectedLanguage = languageCode;
        LanguageSelected?.Invoke(this, languageCode);
    }

    private void UpdateSelection()
    {
        if (EnglishSegment is null || TamilSegment is null ||
            EnglishLabel is null || TamilLabel is null)
            return;

        var englishSelected = NormalizeLanguage(SelectedLanguage) == "en";
        var brandPrimary = GetColorResource("BrandPrimary", Color.FromArgb("#101F50"));
        var selectedText = Colors.White;
        var unselectedBackground = Colors.Transparent;

        EnglishSegment.BackgroundColor = englishSelected ? brandPrimary : unselectedBackground;
        TamilSegment.BackgroundColor = englishSelected ? unselectedBackground : brandPrimary;

        EnglishLabel.TextColor = englishSelected ? selectedText : brandPrimary;
        TamilLabel.TextColor = englishSelected ? brandPrimary : selectedText;

        SemanticProperties.SetDescription(EnglishSegment, "English");
        SemanticProperties.SetDescription(TamilSegment, "Tamil");
        SemanticProperties.SetHint(EnglishSegment, englishSelected ? "Selected" : "Double tap to switch to English");
        SemanticProperties.SetHint(TamilSegment, !englishSelected ? "Selected" : "Double tap to switch to Tamil");
    }

    private static string NormalizeLanguage(string? languageCode) =>
        string.Equals(languageCode, "ta", StringComparison.OrdinalIgnoreCase) ? "ta" : "en";

    private static Color GetColorResource(string key, Color fallback)
    {
        var resources = Microsoft.Maui.Controls.Application.Current?.Resources;
        if (resources is not null && resources.TryGetValue(key, out var value) && value is Color color)
            return color;

        return fallback;
    }
}
