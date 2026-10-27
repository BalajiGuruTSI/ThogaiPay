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

        ApplyLanguage(Preferences.Get("ThogaiPay.Language", "ta"));
    }

    private void OnOtpBoxesTapped(object? sender, TappedEventArgs e)
    {
        OtpInputEntry.Focus();
    }

    private async void OnBackTapped(object? sender, TappedEventArgs e)
    {
        try
        {
            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"OTP page back navigation failed: {ex}");
        }
    }

    private void OnEnglishTapped(object? sender, TappedEventArgs e)
    {
        Preferences.Set("ThogaiPay.Language", "en");
        ApplyLanguage("en");
    }

    private void OnTamilTapped(object? sender, TappedEventArgs e)
    {
        Preferences.Set("ThogaiPay.Language", "ta");
        ApplyLanguage("ta");
    }

    private void ApplyLanguage(string language)
    {
        var tamil = language.Equals("ta", StringComparison.OrdinalIgnoreCase);

        EnglishLanguageBorder.BackgroundColor = tamil ? Colors.White : Color.FromArgb("#0759E8");
        EnglishLanguageLabel.TextColor = tamil ? Color.FromArgb("#536786") : Colors.White;
        EnglishLanguageLabel.FontAttributes = tamil ? FontAttributes.None : FontAttributes.Bold;

        TamilLanguageBorder.BackgroundColor = tamil ? Color.FromArgb("#0759E8") : Colors.White;
        TamilLanguageLabel.TextColor = tamil ? Colors.White : Color.FromArgb("#536786");
        TamilLanguageLabel.FontAttributes = tamil ? FontAttributes.Bold : FontAttributes.None;

        HeadingLabel.Text = tamil ? "OTP சரிபார்ப்பு" : "Verify OTP";
        SubtitleLabel.Text = tamil
            ? "உங்கள் மொபைல் எண்ணிற்கு அனுப்பிய\n6 இலக்க OTP ஐ உள்ளிடுங்கள்"
            : "Enter the 6-digit OTP sent to\nyour mobile number";
        MobileNumberCaption.Text = tamil ? "மொபைல் எண்" : "Mobile number";
        EditNumberLabel.Text = tamil ? "திருத்து" : "Edit";
        ResendCaptionLabel.Text = tamil ? "மீண்டும் அனுப்ப" : "Resend code";
        VerifyActionButton.Text = tamil ? "சரிபார்க்கவும்" : "Verify and continue";
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        static object? GetValueIgnoreCase(IDictionary<string, object> source, string key)
        {
            foreach (var pair in source)
            {
                if (string.Equals(pair.Key, key, StringComparison.OrdinalIgnoreCase))
                    return pair.Value;
            }

            return null;
        }

        var sessionId = GetValueIgnoreCase(query, "sessionId");
        var mobileNumber = GetValueIgnoreCase(query, "mobileNumber");

        _viewModel.Initialize(
            sessionId?.ToString() ?? string.Empty,
            mobileNumber?.ToString() ?? string.Empty);
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        OtpInputEntry.Text = _viewModel.Otp;
    }

    protected override void OnDisappearing()
    {
        _viewModel.StopTimer();
        base.OnDisappearing();
    }
}
