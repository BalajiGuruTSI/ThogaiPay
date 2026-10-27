using ThogaiPay.Controls;

namespace ThogaiPay;

/// <summary>Base for the four main sections. Adds the shared bottom bar once,
/// without requiring a SoftPillBottomBar element in each page XAML file.</summary>
public class MainTabPage : ContentPage
{
    private bool _bottomBarAttached;
    protected virtual string CurrentTab => string.Empty;

    protected override void OnAppearing()
    {
        base.OnAppearing();
        AttachSharedBottomBar();
    }

    private void AttachSharedBottomBar()
    {
        if ( _bottomBarAttached || Content is null ) return;

        var pageContent = Content;
        Content = null;

        var host = new Grid
        {
            RowDefinitions = { new RowDefinition(GridLength.Star), new RowDefinition(GridLength.Auto) },
            RowSpacing = 0,
            BackgroundColor = BackgroundColor
        };
        host.Add(pageContent);
        Grid.SetRow(pageContent, 0);

        var bottomBar = new SoftPillBottomBar { ActiveTab = CurrentTab };
        host.Add(bottomBar);
        Grid.SetRow(bottomBar, 1);

        Content = host;
        _bottomBarAttached = true;
    }
}
