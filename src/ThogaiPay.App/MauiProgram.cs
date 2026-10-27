using Microsoft.Maui;
using Microsoft.Maui.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ThogaiPay.App.Features.Auth.Views;
using ThogaiPay.App.Features.Auth.ViewModels;
using ThogaiPay.App.Shared.Localization;

namespace ThogaiPay.App;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		// Remove Android's native EditText underline so shared entries use the app design system.
		Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping("ThogaiPayBorderlessEntry", (handler, view) =>
		{
			if (OperatingSystem.IsAndroid())
				handler.PlatformView.Background = null;
		});

		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		builder.Services.AddSingleton<ILocalizationService, LocalizationService>();
		builder.Services.AddTransient<LoginViewModel>();
		builder.Services.AddTransient<LoginPage>();
		builder.Services.AddTransient<OtpVerificationPage>();
		builder.Services.AddSingleton<AppShell>();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
