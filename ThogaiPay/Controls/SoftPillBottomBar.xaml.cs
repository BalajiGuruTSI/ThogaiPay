using System.Windows.Input;

namespace ThogaiPay.Controls;

public partial class SoftPillBottomBar : ContentView
{
    private const string ActiveBlue = "#0759D8";
    private const string InactiveSlate = "#667394";
    private const string ActivePill = "#E5F0FF";
    private const string Transparent = "#00FFFFFF";

    private Shell? _subscribedShell;

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

        // Defaults make the component usable without a page-specific ViewModel.
        HomeCommand = new Command(async () => await NavigateToAsync("//MainTabs/MainPage"));
        RechargeCommand = new Command(async () => await NavigateToAsync("//MainTabs/RechargePage"));
        HistoryCommand = new Command(async () => await NavigateToAsync("//MainTabs/HistoryPage"));
        ProfileCommand = new Command(async () => await NavigateToAsync("//MainTabs/ProfilePage"));

        Loaded += OnLoaded;
        Unloaded += OnUnloaded;
    }

    private static async Task NavigateToAsync(string route)
    {
        var shell = Shell.Current;
        if (shell is null)
            return;

        try
        {
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                await shell.GoToAsync(route, animate: false);
            });
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[SoftPillBottomBar] Navigation to '{route}' failed: {ex}");
            await MainThread.InvokeOnMainThreadAsync(async () =>
                await Shell.Current.DisplayAlert("Navigation error", $"Couldn't open this section. Details: {ex.Message}", "OK"));
        }
    }

    private void OnLoaded(object? sender, EventArgs e)
    {
        var shell = Shell.Current;
        if (shell is null)
            return;

        if (_subscribedShell is not null)
            _subscribedShell.Navigated -= OnShellNavigated;

        _subscribedShell = shell;
        _subscribedShell.Navigated += OnShellNavigated;
        UpdateActiveTab(shell.CurrentState.Location.OriginalString);
    }

    private void OnUnloaded(object? sender, EventArgs e)
    {
        if (_subscribedShell is not null)
        {
            _subscribedShell.Navigated -= OnShellNavigated;
            _subscribedShell = null;
        }
    }

    private void OnShellNavigated(object? sender, ShellNavigatedEventArgs e)
    {
        UpdateActiveTab(e.Current.Location.OriginalString);
    }

    private void UpdateActiveTab(string? location)
    {
        if (string.IsNullOrWhiteSpace(location))
            return;

        if (location.Contains("RechargePage", StringComparison.OrdinalIgnoreCase))
            ActiveTab = "Recharge";
        else if (location.Contains("HistoryPage", StringComparison.OrdinalIgnoreCase))
            ActiveTab = "History";
        else if (location.Contains("ProfilePage", StringComparison.OrdinalIgnoreCase))
            ActiveTab = "Profile";
        else if (location.Contains("MainPage", StringComparison.OrdinalIgnoreCase))
            ActiveTab = "Home";
    }

    private bool IsActive(string tab) =>
        string.Equals(ActiveTab, tab, StringComparison.OrdinalIgnoreCase);

    private static void OnActiveTabChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is not SoftPillBottomBar bar)
            return;

        foreach (var property in new[]
        {
            nameof(HomeColor), nameof(RechargeColor), nameof(HistoryColor), nameof(ProfileColor),
            nameof(HomeBackground), nameof(RechargeBackground), nameof(HistoryBackground), nameof(ProfileBackground),
            nameof(HomeOpacity), nameof(RechargeOpacity), nameof(HistoryOpacity), nameof(ProfileOpacity)
        })
        {
            bar.OnPropertyChanged(property);
        }
    }
}
