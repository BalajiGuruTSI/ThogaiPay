using Microsoft.Extensions.Logging;
using Microsoft.Maui.Handlers;
using ThogaiPay.Services.Authentication;
using ThogaiPay.ViewModels.Authentication;
using ThogaiPay.Views.Authentication;
using ThogaiPay.Views.Recharge;

namespace ThogaiPay
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddSingleton<IAuthenticationService, MockAuthenticationService>();

            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddTransient<OtpVerificationViewModel>();

            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<OtpVerificationPage>();
            builder.Services.AddTransient<MobileRechargePage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

#if ANDROID
            EntryHandler.Mapper.AppendToMapping("NoUnderline", (handler, view) =>
            {
                handler.PlatformView.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Transparent);
            });
#endif

            return builder.Build();
        }
    }
}
