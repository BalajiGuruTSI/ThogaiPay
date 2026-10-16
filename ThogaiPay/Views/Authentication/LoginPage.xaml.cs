using ThogaiPay.ViewModels.Authentication;

namespace ThogaiPay.Views.Authentication;

public partial class LoginPage : ContentPage
{
    public LoginPage(LoginViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
