using Microsoft.Extensions.DependencyInjection;
using ThogaiPay.App.Shared.Startup;

namespace ThogaiPay.App;

public partial class App : Microsoft.Maui.Controls.Application
{
    private readonly IServiceProvider _services;

    public App(IServiceProvider services)
    {
        InitializeComponent();
        _services = services;
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        Window? window = null;
        window = new Window(new BrandedSplashPage(() =>
        {
            if (window is not null)
                window.Page = _services.GetRequiredService<AppShell>();
        }));

        return window;
    }
}
