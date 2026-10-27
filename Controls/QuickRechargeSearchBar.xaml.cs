using System.Windows.Input;

namespace ThogaiPay.Controls;

public partial class QuickRechargeSearchBar : ContentView
{
    public static readonly BindableProperty TextProperty = BindableProperty.Create(
        nameof(Text), typeof(string), typeof(QuickRechargeSearchBar), string.Empty,
        BindingMode.TwoWay, propertyChanged: OnTextChanged);

    public static readonly BindableProperty PlaceholderProperty = BindableProperty.Create(
        nameof(Placeholder), typeof(string), typeof(QuickRechargeSearchBar),
        "Enter mobile no. or DTH ID to recharge");

    public static readonly BindableProperty SearchCommandProperty = BindableProperty.Create(
        nameof(SearchCommand), typeof(ICommand), typeof(QuickRechargeSearchBar));

    public static readonly BindableProperty PickContactCommandProperty = BindableProperty.Create(
        nameof(PickContactCommand), typeof(ICommand), typeof(QuickRechargeSearchBar));

    public string Text { get => (string)GetValue(TextProperty); set => SetValue(TextProperty, value); }
    public string Placeholder { get => (string)GetValue(PlaceholderProperty); set => SetValue(PlaceholderProperty, value); }
    public ICommand? SearchCommand { get => (ICommand?)GetValue(SearchCommandProperty); set => SetValue(SearchCommandProperty, value); }
    public ICommand? PickContactCommand { get => (ICommand?)GetValue(PickContactCommandProperty); set => SetValue(PickContactCommandProperty, value); }

    public QuickRechargeSearchBar() => InitializeComponent();

    private static void OnTextChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is QuickRechargeSearchBar control &&
            control.SearchEntry.Text != (newValue as string ?? string.Empty))
            control.SearchEntry.Text = newValue as string ?? string.Empty;
    }

    public void FocusSearch() => SearchEntry.Focus();

    public void ClearSearch()
    {
        Text = string.Empty;
        SearchEntry.Text = string.Empty;
    }
}
