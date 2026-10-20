using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;
using ThogaiPay.Services.Authentication;

namespace ThogaiPay.Views.Startup;

public sealed class SplashPage : ContentPage
{
    private bool _navigated;

    public SplashPage()
    {
        BackgroundColor = Color.FromArgb("#082B59");
        this.On<iOS>().SetUseSafeArea(false);

        Content = new Grid
        {
            Children =
            {
                new Image
                {
                    Source = "thogai_splash.png",
                    Aspect = Aspect.AspectFill,
                    HorizontalOptions = LayoutOptions.Fill,
                    VerticalOptions = LayoutOptions.Fill
                }
            }
        };
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (_navigated)
        {
            return;
        }

        _navigated = true;

        // Keep startup transition short and deterministic.
        await Task.Delay(300);

        var shell = new AppShell();
        var authService = Microsoft.Maui.Controls.Application.Current?.Handler?.MauiContext?.Services.GetService<IAuthenticationService>();

        if (Microsoft.Maui.Controls.Application.Current?.Windows.Count > 0)
        {
            Microsoft.Maui.Controls.Application.Current.Windows[0].Page = shell;
        }

        if (authService?.IsAuthenticated != true)
        {
            await shell.GoToAsync("//MainPage");
            return;
        }

        // LoginPage is shell default content for unauthenticated users.
    }
}
