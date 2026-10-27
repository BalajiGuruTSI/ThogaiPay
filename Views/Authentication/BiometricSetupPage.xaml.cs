using ThogaiPay.ViewModels.Authentication;
namespace ThogaiPay.Views.Authentication;
public partial class BiometricSetupPage : ContentPage { public BiometricSetupPage(BiometricSetupViewModel viewModel) { InitializeComponent(); BindingContext = viewModel; } }
