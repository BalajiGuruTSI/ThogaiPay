using Microsoft.Extensions.DependencyInjection;
using ThogaiPay.Views.Authentication;
using ThogaiPay.Views.Settings;

namespace ThogaiPay;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        // Resolve pages through MAUI DI so their ViewModels/services are injected.
        var services = Application.Current?.Handler?.MauiContext?.Services;
        if (services is not null)
        {
            LoginShellContent.ContentTemplate = new DataTemplate(
                () => services.GetRequiredService<LoginPage>());
        }

        Routing.RegisterRoute(nameof(OtpVerificationPage), typeof(OtpVerificationPage));
        Routing.RegisterRoute(nameof(PinSetupPage), typeof(PinSetupPage));
        Routing.RegisterRoute(nameof(PinUnlockPage), typeof(PinUnlockPage));
        Routing.RegisterRoute(nameof(BiometricSetupPage), typeof(BiometricSetupPage));
        Routing.RegisterRoute(nameof(SettingsPage), typeof(SettingsPage));
    }
}
