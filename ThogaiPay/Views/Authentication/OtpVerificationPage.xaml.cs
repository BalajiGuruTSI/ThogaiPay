using ThogaiPay.ViewModels.Authentication;

namespace ThogaiPay.Views.Authentication;

public partial class OtpVerificationPage : ContentPage, IQueryAttributable
{
    private readonly OtpVerificationViewModel _viewModel;

    public OtpVerificationPage(OtpVerificationViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        MainThread.BeginInvokeOnMainThread(() => OtpInputEntry.Focus());
    }

    private void OnOtpBoxesTapped(object? sender, TappedEventArgs e)
    {
        OtpInputEntry.Focus();
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        static object? GetValueIgnoreCase(IDictionary<string, object> source, string key)
        {
            foreach (var pair in source)
            {
                if (string.Equals(pair.Key, key, StringComparison.OrdinalIgnoreCase))
                {
                    return pair.Value;
                }
            }

            return null;
        }

        var sessionId = GetValueIgnoreCase(query, "sessionId");
        var mobileNumber = GetValueIgnoreCase(query, "mobileNumber");

        _viewModel.Initialize(sessionId?.ToString() ?? string.Empty, mobileNumber?.ToString() ?? string.Empty);
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _viewModel.StopTimer();
    }
}
