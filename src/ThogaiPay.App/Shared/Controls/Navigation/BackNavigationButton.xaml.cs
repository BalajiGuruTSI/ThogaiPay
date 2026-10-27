using Microsoft.Maui.Controls;

namespace ThogaiPay.App.Shared.Controls.Navigation;

public partial class BackNavigationButton : ContentView
{
    public event EventHandler? Clicked;

    public BackNavigationButton()
    {
        InitializeComponent();
        SemanticProperties.SetDescription(this, "Go back");
        SemanticProperties.SetHint(this, "Returns to the previous screen");
    }

    private void OnTapped(object sender, TappedEventArgs e) => Clicked?.Invoke(this, EventArgs.Empty);
}
