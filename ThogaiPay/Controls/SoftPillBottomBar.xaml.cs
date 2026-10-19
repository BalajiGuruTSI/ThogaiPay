using System.Windows.Input;

namespace ThogaiPay.Controls;

public partial class SoftPillBottomBar : ContentView
{
    private const string ActiveBlue = "#0759D8";
    private const string InactiveSlate = "#667394";
    private const string ActivePill = "#E5F0FF";
    private const string Transparent = "#00FFFFFF";

    public static readonly BindableProperty ActiveTabProperty = BindableProperty.Create(
        nameof(ActiveTab), typeof(string), typeof(SoftPillBottomBar), "Home",
        propertyChanged: OnActiveTabChanged);

    public static readonly BindableProperty HomeCommandProperty = CreateCommandProperty(nameof(HomeCommand));
    public static readonly BindableProperty RechargeCommandProperty = CreateCommandProperty(nameof(RechargeCommand));
    public static readonly BindableProperty HistoryCommandProperty = CreateCommandProperty(nameof(HistoryCommand));
    public static readonly BindableProperty ProfileCommandProperty = CreateCommandProperty(nameof(ProfileCommand));

    private static BindableProperty CreateCommandProperty(string name) =>
        BindableProperty.Create(name, typeof(ICommand), typeof(SoftPillBottomBar));

    public string ActiveTab
    {
        get => (string)GetValue(ActiveTabProperty);
        set => SetValue(ActiveTabProperty, value);
    }

    public ICommand? HomeCommand
    {
        get => (ICommand?)GetValue(HomeCommandProperty);
        set => SetValue(HomeCommandProperty, value);
    }

    public ICommand? RechargeCommand
    {
        get => (ICommand?)GetValue(RechargeCommandProperty);
        set => SetValue(RechargeCommandProperty, value);
    }

    public ICommand? HistoryCommand
    {
        get => (ICommand?)GetValue(HistoryCommandProperty);
        set => SetValue(HistoryCommandProperty, value);
    }

    public ICommand? ProfileCommand
    {
        get => (ICommand?)GetValue(ProfileCommandProperty);
        set => SetValue(ProfileCommandProperty, value);
    }

    public Color HomeColor => IsActive("Home") ? Color.FromArgb(ActiveBlue) : Color.FromArgb(InactiveSlate);
    public Color RechargeColor => IsActive("Recharge") ? Color.FromArgb(ActiveBlue) : Color.FromArgb(InactiveSlate);
    public Color HistoryColor => IsActive("History") ? Color.FromArgb(ActiveBlue) : Color.FromArgb(InactiveSlate);
    public Color ProfileColor => IsActive("Profile") ? Color.FromArgb(ActiveBlue) : Color.FromArgb(InactiveSlate);

    public Color HomeBackground => IsActive("Home") ? Color.FromArgb(ActivePill) : Color.FromArgb(Transparent);
    public Color RechargeBackground => IsActive("Recharge") ? Color.FromArgb(ActivePill) : Color.FromArgb(Transparent);
    public Color HistoryBackground => IsActive("History") ? Color.FromArgb(ActivePill) : Color.FromArgb(Transparent);
    public Color ProfileBackground => IsActive("Profile") ? Color.FromArgb(ActivePill) : Color.FromArgb(Transparent);

    public double HomeOpacity => IsActive("Home") ? 1 : 0.72;
    public double RechargeOpacity => IsActive("Recharge") ? 1 : 0.72;
    public double HistoryOpacity => IsActive("History") ? 1 : 0.72;
    public double ProfileOpacity => IsActive("Profile") ? 1 : 0.72;

    public SoftPillBottomBar()
    {
        InitializeComponent();
    }

    private bool IsActive(string tab) =>
        string.Equals(ActiveTab, tab, StringComparison.OrdinalIgnoreCase);

    private static void OnActiveTabChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is not SoftPillBottomBar bar)
            return;

        bar.OnPropertyChanged(nameof(HomeColor));
        bar.OnPropertyChanged(nameof(RechargeColor));
        bar.OnPropertyChanged(nameof(HistoryColor));
        bar.OnPropertyChanged(nameof(ProfileColor));

        bar.OnPropertyChanged(nameof(HomeBackground));
        bar.OnPropertyChanged(nameof(RechargeBackground));
        bar.OnPropertyChanged(nameof(HistoryBackground));
        bar.OnPropertyChanged(nameof(ProfileBackground));

        bar.OnPropertyChanged(nameof(HomeOpacity));
        bar.OnPropertyChanged(nameof(RechargeOpacity));
        bar.OnPropertyChanged(nameof(HistoryOpacity));
        bar.OnPropertyChanged(nameof(ProfileOpacity));
    }
}
