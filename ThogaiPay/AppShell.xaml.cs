
using ThogaiPay.Views.Authentication;

namespace ThogaiPay;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        // Register authentication routes used before the main tab layout.
        Routing.RegisterRoute(
            nameof(LoginPage),
            typeof(LoginPage));

        Routing.RegisterRoute(
            nameof(OtpVerificationPage),
            typeof(OtpVerificationPage));
    }
}
