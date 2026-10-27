namespace ThogaiPay.Controls;

public partial class HomeRecentRechargesSection : ContentView
{
    public HomeRecentRechargesSection() => InitializeComponent();

    private async void OnViewAllTapped(object? sender, TappedEventArgs e)
    {
        try
        {
            await Shell.Current.GoToAsync("//MainTabs/HistoryPage");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Unable to open recharge history: {ex.Message}");
        }
    }
}
