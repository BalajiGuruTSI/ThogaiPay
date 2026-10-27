using ThogaiPay.ViewModels.Authentication;
namespace ThogaiPay.Views.Authentication;
public partial class PinUnlockPage : ContentPage { public PinUnlockPage(PinUnlockViewModel viewModel) { InitializeComponent(); BindingContext = viewModel; } }
