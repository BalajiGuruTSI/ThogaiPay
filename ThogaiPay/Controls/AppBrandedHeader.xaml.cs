using System.Windows.Input;

namespace ThogaiPay.Controls;

public partial class AppBrandedHeader : ContentView
{
    public static readonly BindableProperty TitleTextProperty = BindableProperty.Create(
        nameof(TitleText),
        typeof(string),
        typeof(AppBrandedHeader),
        "Thogai");

    public static readonly BindableProperty AccentTitleTextProperty = BindableProperty.Create(
        nameof(AccentTitleText),
        typeof(string),
        typeof(AppBrandedHeader),
        "Pay");

    public static readonly BindableProperty SubtitleTextProperty = BindableProperty.Create(
        nameof(SubtitleText),
        typeof(string),
        typeof(AppBrandedHeader),
        "நம் ஊர் · நம் மக்கள் · நம் நம்பிக்கை");

    public static readonly BindableProperty ShowProfileButtonProperty = BindableProperty.Create(
        nameof(ShowProfileButton),
        typeof(bool),
        typeof(AppBrandedHeader),
        true);

    public static readonly BindableProperty ProfileCommandProperty = BindableProperty.Create(
        nameof(ProfileCommand),
        typeof(ICommand),
        typeof(AppBrandedHeader));

    public string TitleText
    {
        get => (string)GetValue(TitleTextProperty);
        set => SetValue(TitleTextProperty, value);
    }

    public string AccentTitleText
    {
        get => (string)GetValue(AccentTitleTextProperty);
        set => SetValue(AccentTitleTextProperty, value);
    }

    public string SubtitleText
    {
        get => (string)GetValue(SubtitleTextProperty);
        set => SetValue(SubtitleTextProperty, value);
    }

    public bool ShowProfileButton
    {
        get => (bool)GetValue(ShowProfileButtonProperty);
        set => SetValue(ShowProfileButtonProperty, value);
    }

    public ICommand? ProfileCommand
    {
        get => (ICommand?)GetValue(ProfileCommandProperty);
        set => SetValue(ProfileCommandProperty, value);
    }

    public AppBrandedHeader()
    {
        InitializeComponent();
    }
}
