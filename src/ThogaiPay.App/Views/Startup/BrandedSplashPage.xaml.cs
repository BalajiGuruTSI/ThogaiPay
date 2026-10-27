using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using ThogaiPay.App.Branding.Generated;

namespace ThogaiPay.App.Views.Startup;

/// <summary>
/// Shows the selected brand's complete logo lockup over the configured splash background,
/// then hands control to the main application shell.
/// </summary>
public partial class BrandedSplashPage : ContentPage
{
    private readonly Action _continueToApp;
    private bool _hasStarted;

    public BrandedSplashPage(Action continueToApp)
    {
        _continueToApp = continueToApp
            ?? throw new ArgumentNullException(nameof(continueToApp));

        InitializeComponent();

        // The generated asset is the complete brand lockup image: symbol, wordmark, and Tamil tagline together.
        BrandLogo.Source = ImageSource.FromFile(BrandIdentity.LogoFileName);
        SemanticProperties.SetDescription(
            BrandLogo,
            $"{BrandIdentity.DisplayName} logo and tagline");
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (_hasStarted)
            return;

        _hasStarted = true;

        try
        {
            await Task.WhenAll(
                BrandLogo.FadeTo(1, 700, Easing.CubicOut),
                BrandLogo.ScaleTo(1, 850, Easing.SpringOut),
                BrandLogo.TranslateTo(0, 0, 700, Easing.CubicOut));

            // Keep the complete logo lockup visible briefly, without delaying startup excessively.
            await Task.Delay(1200);
            await SplashRoot.FadeTo(0, 250, Easing.CubicIn);
        }
        catch (OperationCanceledException)
        {
            // Platform lifecycle changes may interrupt animations; startup must still continue.
        }
        finally
        {
            MainThread.BeginInvokeOnMainThread(_continueToApp);
        }
    }
}
