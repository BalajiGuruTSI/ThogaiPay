using System.Windows.Input;

namespace ThogaiPay.Controls;

public partial class AppBottomBar : ContentView
{
    public static readonly BindableProperty ActiveTabProperty = BindableProperty.Create(
        nameof(ActiveTab),
        typeof(string),
        typeof(AppBottomBar),
        "Home",
        propertyChanged: OnActiveTabChanged);

    public static readonly BindableProperty HomeCommandProperty = BindableProperty.Create(
        nameof(HomeCommand),
        typeof(ICommand),
        typeof(AppBottomBar));

    public static readonly BindableProperty RechargeCommandProperty = BindableProperty.Create(
        nameof(RechargeCommand),
        typeof(ICommand),
        typeof(AppBottomBar));

    public static readonly BindableProperty HistoryCommandProperty = BindableProperty.Create(
        nameof(HistoryCommand),
        typeof(ICommand),
        typeof(AppBottomBar));

    public static readonly BindableProperty ProfileCommandProperty = BindableProperty.Create(
        nameof(ProfileCommand),
        typeof(ICommand),
        typeof(AppBottomBar));

    public static readonly BindableProperty CenterCommandProperty = BindableProperty.Create(
        nameof(CenterCommand),
        typeof(ICommand),
        typeof(AppBottomBar));

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

    public ICommand? CenterCommand
    {
        get => (ICommand?)GetValue(CenterCommandProperty);
        set => SetValue(CenterCommandProperty, value);
    }

    public Color HomeTabColor => IsActive("Home") ? Color.FromArgb("#0759D8") : Color.FromArgb("#66799A");

    public Color RechargeTabColor => IsActive("Recharge") ? Color.FromArgb("#0759D8") : Color.FromArgb("#66799A");

    public Color HistoryTabColor => IsActive("History") ? Color.FromArgb("#0759D8") : Color.FromArgb("#66799A");

    public Color ProfileTabColor => IsActive("Profile") ? Color.FromArgb("#0759D8") : Color.FromArgb("#66799A");

    public double HomeTabIconOpacity => IsActive("Home") ? 1 : 0.58;

    public double RechargeTabIconOpacity => IsActive("Recharge") ? 1 : 0.58;

    public double HistoryTabIconOpacity => IsActive("History") ? 1 : 0.58;

    public double ProfileTabIconOpacity => IsActive("Profile") ? 1 : 0.58;

    public AppBottomBar()
    {
        InitializeComponent();
    }

    private static void OnActiveTabChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is not AppBottomBar bottomBar)
        {
            return;
        }

        bottomBar.OnPropertyChanged(nameof(HomeTabColor));
        bottomBar.OnPropertyChanged(nameof(RechargeTabColor));
        bottomBar.OnPropertyChanged(nameof(HistoryTabColor));
        bottomBar.OnPropertyChanged(nameof(ProfileTabColor));
        bottomBar.OnPropertyChanged(nameof(HomeTabIconOpacity));
        bottomBar.OnPropertyChanged(nameof(RechargeTabIconOpacity));
        bottomBar.OnPropertyChanged(nameof(HistoryTabIconOpacity));
        bottomBar.OnPropertyChanged(nameof(ProfileTabIconOpacity));
    }

    private bool IsActive(string tabName)
    {
        return string.Equals(ActiveTab, tabName, StringComparison.OrdinalIgnoreCase);
    }
}
