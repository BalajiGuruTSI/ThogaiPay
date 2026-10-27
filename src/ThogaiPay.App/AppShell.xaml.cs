using Microsoft.Maui.Controls;
using ThogaiPay.App.Features.Auth.Views;

namespace ThogaiPay.App;

public partial class AppShell : Shell
{
    public AppShell(LoginPage loginPage)
    {
        InitializeComponent();
        Title = Microsoft.Maui.ApplicationModel.AppInfo.Current.Name;

        Items.Add(new ShellContent
        {
            Title = "Login",
            Route = "login",
            Content = loginPage
        });

        Routing.RegisterRoute("otp", typeof(OtpVerificationPage));
    }
}
