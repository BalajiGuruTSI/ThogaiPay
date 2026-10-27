
using System.Windows.Input;

namespace ThogaiPay.Controls;

public partial class AppBrandedHeader : ContentView
{
    public static readonly BindableProperty HeaderImageSourceProperty =
        BindableProperty.Create(
            nameof(HeaderImageSource),
            typeof(ImageSource),
            typeof(AppBrandedHeader),
            defaultValue: ImageSource.FromFile("header_home.png"));

    public static readonly BindableProperty LogoSourceProperty =
        BindableProperty.Create(
            nameof(LogoSource),
            typeof(ImageSource),
            typeof(AppBrandedHeader),
            defaultValue: ImageSource.FromFile("thogaipay_mark_brand.svg"));

    public static readonly BindableProperty ShowBrandProperty =
        BindableProperty.Create(
            nameof(ShowBrand),
            typeof(bool),
            typeof(AppBrandedHeader),
            true);

    public static readonly BindableProperty ShowBackButtonProperty =
        BindableProperty.Create(
            nameof(ShowBackButton),
            typeof(bool),
            typeof(AppBrandedHeader),
            false);

    public static readonly BindableProperty ShowProfileButtonProperty =
        BindableProperty.Create(
            nameof(ShowProfileButton),
            typeof(bool),
            typeof(AppBrandedHeader),
            false);

    public static readonly BindableProperty ShowPageTitleProperty =
        BindableProperty.Create(
            nameof(ShowPageTitle),
            typeof(bool),
            typeof(AppBrandedHeader),
            false);

    public static readonly BindableProperty PageTitleProperty =
        BindableProperty.Create(
            nameof(PageTitle),
            typeof(string),
            typeof(AppBrandedHeader),
            string.Empty);

    public static readonly BindableProperty SubtitleProperty =
        BindableProperty.Create(
            nameof(Subtitle),
            typeof(string),
            typeof(AppBrandedHeader),
            string.Empty);

    public static readonly BindableProperty BackCommandProperty =
        BindableProperty.Create(
            nameof(BackCommand),
            typeof(ICommand),
            typeof(AppBrandedHeader));

    public static readonly BindableProperty ProfileCommandProperty =
        BindableProperty.Create(
            nameof(ProfileCommand),
            typeof(ICommand),
            typeof(AppBrandedHeader));

    public ImageSource HeaderImageSource
    {
        get => (ImageSource)GetValue(HeaderImageSourceProperty);
        set => SetValue(HeaderImageSourceProperty, value);
    }

    public ImageSource LogoSource
    {
        get => (ImageSource)GetValue(LogoSourceProperty);
        set => SetValue(LogoSourceProperty, value);
    }

    public bool ShowBrand
    {
        get => (bool)GetValue(ShowBrandProperty);
        set => SetValue(ShowBrandProperty, value);
    }

    public bool ShowBackButton
    {
        get => (bool)GetValue(ShowBackButtonProperty);
        set => SetValue(ShowBackButtonProperty, value);
    }

    public bool ShowProfileButton
    {
        get => (bool)GetValue(ShowProfileButtonProperty);
        set => SetValue(ShowProfileButtonProperty, value);
    }

    public bool ShowPageTitle
    {
        get => (bool)GetValue(ShowPageTitleProperty);
        set => SetValue(ShowPageTitleProperty, value);
    }

    public string PageTitle
    {
        get => (string)GetValue(PageTitleProperty);
        set => SetValue(PageTitleProperty, value);
    }

    public string Subtitle
    {
        get => (string)GetValue(SubtitleProperty);
        set => SetValue(SubtitleProperty, value);
    }

    public ICommand? BackCommand
    {
        get => (ICommand?)GetValue(BackCommandProperty);
        set => SetValue(BackCommandProperty, value);
    }

    public ICommand? ProfileCommand
    {
        get => (ICommand?)GetValue(ProfileCommandProperty);
        set => SetValue(ProfileCommandProperty, value);
    }

    public AppBrandedHeader()
    {
        InitializeComponent();

        // Default back behaviour can be overridden by the calling page.
        BackCommand = new Command(async () => await NavigateBackAsync());
    }

    private static async Task NavigateBackAsync()
    {
        try
        {
            if ( Shell.Current is not null )
            {
                await Shell.Current.GoToAsync("..");
            }
        }
        catch ( InvalidOperationException )
        {
            // Fall back to the navigation stack if Shell navigation
            // is unavailable in the current page context.
            var navigation = Application.Current?.Windows
                .FirstOrDefault()?.Page?.Navigation;

            if ( navigation?.NavigationStack.Count > 1 )
                await navigation.PopAsync();
        }
    }
}
