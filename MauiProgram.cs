using Microsoft.Maui;
using Microsoft.Maui.Hosting;
﻿using Microsoft.Extensions.Logging;
using Microsoft.Maui.Handlers;
using ThogaiPay.Services.Authentication;
using ThogaiPay.ViewModels.Authentication;
using ThogaiPay.Views.Authentication;
using ThogaiPay.Views.Recharge;
using ThogaiPay.Services.Recharge;
using ThogaiPay.Services.Configuration;

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

            // The mock is intentionally registered only for development. Production needs an API-backed implementation.
#if DEBUG
            builder.Services.AddSingleton<IAuthenticationService, MockAuthenticationService>();
#else
            builder.Services.AddSingleton<IAuthenticationService, UnavailableAuthenticationService>();
#endif
            builder.Services.AddSingleton<IPinAuthenticationService, SecurePinAuthenticationService>();
            builder.Services.AddSingleton<IBiometricAuthenticationService, PlatformBiometricAuthenticationService>();
            builder.Services.AddSingleton<ILocalizationService, LocalizationService>();
            builder.Services.AddSingleton<MockRechargeDataService>();
            builder.Services.AddSingleton<IFavouriteRecipientService>(sp => sp.GetRequiredService<MockRechargeDataService>());
            builder.Services.AddSingleton<IRecentRechargeService>(sp => sp.GetRequiredService<MockRechargeDataService>());
            builder.Services.AddSingleton<IRechargeService>(sp => sp.GetRequiredService<MockRechargeDataService>());

            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddTransient<OtpVerificationViewModel>();
            builder.Services.AddTransient<PinSetupViewModel>();
            builder.Services.AddTransient<PinUnlockViewModel>();
            builder.Services.AddTransient<BiometricSetupViewModel>();

            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<OtpVerificationPage>();
            builder.Services.AddTransient<PinSetupPage>();
            builder.Services.AddTransient<PinUnlockPage>();
            builder.Services.AddTransient<BiometricSetupPage>();
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
