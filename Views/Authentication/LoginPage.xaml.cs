using ThogaiPay.ViewModels.Authentication;

namespace ThogaiPay.Views.Authentication;

public partial class LoginPage : ContentPage
{
    public LoginPage(LoginViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    private void OnEnglishTapped(object? sender, TappedEventArgs e)
    {
        if ( BindingContext is LoginViewModel vm )
            vm.SelectedLanguage = "English";
    }

    private void OnTamilTapped(object? sender, TappedEventArgs e)
    {
        if ( BindingContext is LoginViewModel vm )
            vm.SelectedLanguage = "தமிழ்";
    }
}