using Microsoft.Extensions.DependencyInjection;
using ThogaiPay.Services.Authentication;

namespace ThogaiPay.Views.Startup;

public sealed class SplashPage : ContentPage
{
    private bool _navigationStarted;
    private readonly Image _brandLogo;

    public SplashPage()
    {
        BackgroundColor = Color.FromArgb("#F7FAFF");
        Shell.SetNavBarIsVisible(this, false);

        _brandLogo = new Image
        {
            Source = "thogaipay_brand_vertical_logo.png",
            Aspect = Aspect.AspectFit,
            WidthRequest = 330,
            HeightRequest = 390,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
            Opacity = 0,
            Scale = .72,
            Rotation = -4,
            InputTransparent = true
        };
        Content = new Grid
        {
            Children =
            {
                _brandLogo
            }
        };
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (_navigationStarted) return;
        _navigationStarted = true;

        await Task.WhenAll(
            _brandLogo.FadeToAsync(1, 650, Easing.CubicOut),
            _brandLogo.ScaleToAsync(1, 850, Easing.SpringOut),
            _brandLogo.RotateToAsync(0, 650, Easing.CubicOut));
        await Task.Delay(TimeSpan.FromMilliseconds(550));

        var app = Application.Current;
        var window = app?.Windows.FirstOrDefault();
        if (app is null || window is null) return;

        var services = app.Handler?.MauiContext?.Services;
        var authService = services?.GetService<IAuthenticationService>();
        var pinService = services?.GetService<IPinAuthenticationService>();
        var destination = authService?.IsAuthenticated == true
            ? "//MainTabs/MainPage"
            : pinService is not null && await pinService.HasPinAsync()
                ? nameof(Views.Authentication.PinUnlockPage)
                : "//LoginPage";
        var shell = new AppShell();
        window.Page = shell;
        await Task.Delay(100);
        await shell.GoToAsync(destination);
    }
}
