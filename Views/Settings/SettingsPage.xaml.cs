namespace ThogaiPay.Views.Settings;

public partial class SettingsPage : ContentPage
{
    public SettingsPage()
    {
        InitializeComponent();
        LanguagePicker.ItemsSource = new[] { "English", "தமிழ்" };
        LanguagePicker.SelectedIndex = 0;
    }

    private async void OnBackTapped(object sender, TappedEventArgs e)
    {
        if (Shell.Current is not null)
            await Shell.Current.GoToAsync("..");
    }
}
