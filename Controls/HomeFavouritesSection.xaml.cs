namespace ThogaiPay.Controls;

public partial class HomeFavouritesSection : ContentView
{
    public HomeFavouritesSection() => InitializeComponent();

    private async void OnAddFavouriteTapped(object? sender, TappedEventArgs e)
    {
        if (Shell.Current is not null)
            await Shell.Current.GoToAsync("AddFavouritePage");
    }

    private async void OnViewAllTapped(object? sender, TappedEventArgs e)
    {
        if (Shell.Current is not null)
            await Shell.Current.GoToAsync("FavouritesPage");
    }
}