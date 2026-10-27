using ThogaiPay.Views.History;
using ThogaiPay.Views.Recharge;
using ThogaiPay.Views.Settings;

namespace ThogaiPay.Views.Profile;

public partial class ProfilePage : MainTabPage
{
    protected override string CurrentTab => "Profile";

    public ProfilePage() => InitializeComponent();

    private async void OnSettingsTapped(object sender, TappedEventArgs e)
    {
        try
        {
            await MainThread.InvokeOnMainThreadAsync(
                () => Shell.Current.GoToAsync(nameof(SettingsPage)));
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Settings navigation failed: {ex}");
            await MainThread.InvokeOnMainThreadAsync(
                () => Shell.Current.DisplayAlert("Settings", "Unable to open Settings. Please try again.", "OK"));
        }
    }

    private async void OnHistoryTapped(object sender, TappedEventArgs e)
    {
        try { await Shell.Current.GoToAsync("//MainTabs/HistoryPage"); }
        catch (Exception ex) { System.Diagnostics.Debug.WriteLine($"History navigation failed: {ex}"); }
    }

    private async void OnFavouritesTapped(object sender, TappedEventArgs e)
    {
        try { await Shell.Current.GoToAsync("//MainTabs/MainPage"); }
        catch (Exception ex) { System.Diagnostics.Debug.WriteLine($"Favourites navigation failed: {ex}"); }
    }
}
