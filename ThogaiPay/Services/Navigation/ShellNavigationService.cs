using Microsoft.Maui.Controls;

namespace ThogaiPay.Navigation;

/// <summary>
/// Central route navigation for the custom Soft Pill bottom bar.
/// Route names must match AppShell.xaml.
/// </summary>
public sealed class ShellNavigationService
{
    public Task GoHomeAsync() =>
        Shell.Current.GoToAsync("//MainTabs/MainPage");

    public Task GoRechargeAsync() =>
        Shell.Current.GoToAsync("//MainTabs/RechargePage");

    public Task GoHistoryAsync() =>
        Shell.Current.GoToAsync("//MainTabs/HistoryPage");

    public Task GoProfileAsync() =>
        Shell.Current.GoToAsync("//MainTabs/ProfilePage");
}
