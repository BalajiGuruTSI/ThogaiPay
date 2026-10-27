using ThogaiPay.ViewModels.Authentication;
namespace ThogaiPay.Views.Authentication;
public partial class PinSetupPage : ContentPage { public PinSetupPage(PinSetupViewModel viewModel) { InitializeComponent(); BindingContext = viewModel; } }
