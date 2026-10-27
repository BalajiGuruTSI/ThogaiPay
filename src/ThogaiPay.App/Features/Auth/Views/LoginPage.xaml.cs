using Microsoft.Maui.Controls;
using ThogaiPay.App.Features.Auth.ViewModels;
using ThogaiPay.App.Shared.Localization;

namespace ThogaiPay.App.Features.Auth.Views;

public partial class LoginPage : ContentPage
{
    private readonly ILocalizationService _localization;

    public LoginPage(LoginViewModel viewModel, ILocalizationService localization)
    {
        InitializeComponent();
        _localization = localization;
        BindingContext = viewModel;
        LanguageSwitcher.SelectedLanguage = _localization.CurrentLanguage;
        LanguageSwitcher.LanguageSelected += OnLanguageSelected;
        _localization.LanguageChanged += OnGlobalLanguageChanged;
    }

    private void OnLanguageSelected(object? sender, string languageCode) =>
        _localization.SetLanguage(languageCode);

    private void OnGlobalLanguageChanged(object? sender, EventArgs e) =>
        LanguageSwitcher.SelectedLanguage = _localization.CurrentLanguage;

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _localization.LanguageChanged -= OnGlobalLanguageChanged;
    }
}
