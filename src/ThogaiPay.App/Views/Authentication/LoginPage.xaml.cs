using Microsoft.Maui.Controls;
using ThogaiPay.App.ViewModels.Authentication;

namespace ThogaiPay.App.Views.Authentication;

public partial class LoginPage : ContentPage
{
    private readonly LoginViewModel _viewModel;

    public LoginPage(LoginViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    private void OnLanguageChanged(object? sender, EventArgs e)
    {
        if (LanguagePicker.SelectedItem is string language)
            _viewModel.LanguageChangedCommand.Execute(language);
    }
}
