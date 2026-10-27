using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace ThogaiPay.App.Features.Auth.Views;

[QueryProperty(nameof(MobileNumber), "mobileNumber")]
public partial class OtpVerificationPage : ContentPage
{
    private string _mobileNumber = string.Empty;
    private string _otp = string.Empty;
    private string _message = string.Empty;
    private bool _isVerifying;
    private int _secondsRemaining = 30;
    private CancellationTokenSource? _countdownCts;
    private Entry[] _otpEntries = Array.Empty<Entry>();
    private Border[] _otpBorders = Array.Empty<Border>();

    public OtpVerificationPage()
    {
        InitializeComponent();
        _otpEntries = new[] { OtpBox1, OtpBox2, OtpBox3, OtpBox4, OtpBox5, OtpBox6 };
        _otpBorders = new[] { OtpBoxBorder1, OtpBoxBorder2, OtpBoxBorder3, OtpBoxBorder4, OtpBoxBorder5, OtpBoxBorder6 };
        VerifyCommand = new Command(async () => await VerifyAsync(), () => !IsVerifying);
        BindingContext = this;
        StartCountdown();
    }

    public string MobileNumber
    {
        get => _mobileNumber;
        set
        {
            var digits = new string((value ?? string.Empty).Where(char.IsDigit).ToArray());
            _mobileNumber = digits.Length > 10 ? digits[^10..] : digits;
            OnPropertyChanged();
            OnPropertyChanged(nameof(MaskedMobileNumber));
        }
    }

    public string MaskedMobileNumber => _mobileNumber.Length == 10
        ? $"+91 {_mobileNumber[..5]} {_mobileNumber[5..]}"
        : $"+91 {_mobileNumber}";

    public string Otp
    {
        get => _otp;
        private set
        {
            var digits = new string((value ?? string.Empty).Where(char.IsDigit).Take(6).ToArray());
            if (_otp == digits) return;
            _otp = digits;
            OnPropertyChanged();
            Message = string.Empty;
        }
    }

    private void SyncOtpFromBoxes()
    {
        Otp = string.Concat(_otpEntries.Select(entry => entry.Text ?? string.Empty));
    }

    private void OnOtpDigitChanged(object sender, TextChangedEventArgs e)
    {
        if (sender is not Entry entry) return;

        // Keep each cell numeric and single-character. If an OS paste/autofill supplies
        // multiple digits, distribute them across the remaining cells.
        var digits = new string((e.NewTextValue ?? string.Empty).Where(char.IsDigit).ToArray());
        if (digits.Length > 1)
        {
            var start = Array.IndexOf(_otpEntries, entry);
            for (var i = 0; i < digits.Length && start + i < _otpEntries.Length; i++)
                _otpEntries[start + i].Text = digits[i].ToString();
            return;
        }

        if (entry.Text != digits)
        {
            entry.Text = digits;
            return;
        }

        SyncOtpFromBoxes();

        if (digits.Length == 1)
        {
            var index = Array.IndexOf(_otpEntries, entry);
            if (index >= 0 && index < _otpEntries.Length - 1)
                _otpEntries[index + 1].Focus();
        }
    }

    private void OnOtpDigitFocused(object sender, FocusEventArgs e) => UpdateOtpFocus(sender as Entry, true);
    private void OnOtpDigitUnfocused(object sender, FocusEventArgs e) => UpdateOtpFocus(sender as Entry, false);

    private void UpdateOtpFocus(Entry? entry, bool focused)
    {
        var index = entry is null ? -1 : Array.IndexOf(_otpEntries, entry);
        if (index < 0) return;
        _otpBorders[index].Stroke = new SolidColorBrush(Color.FromArgb(focused ? "#0759E8" : "#9EADC6"));
        _otpBorders[index].StrokeThickness = focused ? 2.4 : 1.4;
    }

    private void OnOtpDigitCompleted(object sender, EventArgs e)
    {
        if (sender is not Entry entry) return;
        var index = Array.IndexOf(_otpEntries, entry);
        if (index >= 0 && index < _otpEntries.Length - 1)
            _otpEntries[index + 1].Focus();
    }

    private void OnBackButtonClicked(object sender, EventArgs e) => NavigateBack();
    private void OnEditNumberClicked(object sender, TappedEventArgs e) => NavigateBack();

    private void NavigateBack()
    {
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            try { await Shell.Current.GoToAsync(".."); }
            catch (Exception ex) { System.Diagnostics.Debug.WriteLine($"OTP back navigation failed: {ex}"); }
        });
    }

    private void OnResendClicked(object sender, TappedEventArgs e)
    {
        if (_secondsRemaining > 0) return;
        foreach (var entry in _otpEntries) entry.Text = string.Empty;
        SyncOtpFromBoxes();
        Message = "Demo OTP is 123456.";
        StartCountdown();
        _otpEntries[0].Focus();
    }

    private void StartCountdown()
    {
        _countdownCts?.Cancel();
        _countdownCts?.Dispose();
        _countdownCts = new CancellationTokenSource();
        _secondsRemaining = 30;
        OnPropertyChanged(nameof(CountdownText));
        OnPropertyChanged(nameof(ResendTextColor));
        _ = RunCountdownAsync(_countdownCts.Token);
    }

    private async Task RunCountdownAsync(CancellationToken token)
    {
        try
        {
            while (_secondsRemaining > 0 && !token.IsCancellationRequested)
            {
                await Task.Delay(1000, token);
                await MainThread.InvokeOnMainThreadAsync(() =>
                {
                    _secondsRemaining = Math.Max(0, _secondsRemaining - 1);
                    OnPropertyChanged(nameof(CountdownText));
                    OnPropertyChanged(nameof(ResendTextColor));
                });
            }
        }
        catch (OperationCanceledException) { }
    }

    public string CountdownText => $"00:{_secondsRemaining:D2}";
    public Color ResendTextColor => _secondsRemaining == 0
        ? Color.FromArgb("#0759E8")
        : Color.FromArgb("#8793A8");

    public string Message
    {
        get => _message;
        private set
        {
            if (_message == value) return;
            _message = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(HasMessage));
            OnPropertyChanged(nameof(MessageColor));
        }
    }

    public bool HasMessage => !string.IsNullOrWhiteSpace(Message);
    public Color MessageColor => Message.StartsWith("Incorrect", StringComparison.OrdinalIgnoreCase)
        || Message.StartsWith("Enter", StringComparison.OrdinalIgnoreCase)
        ? Color.FromArgb("#DC2626")
        : Color.FromArgb("#16A34A");

    public bool IsVerifying
    {
        get => _isVerifying;
        private set
        {
            if (_isVerifying == value) return;
            _isVerifying = value;
            OnPropertyChanged();
            VerifyCommand.ChangeCanExecute();
        }
    }

    public Command VerifyCommand { get; }

    private async Task VerifyAsync()
    {
        if (IsVerifying) return;
        SyncOtpFromBoxes();
        if (Otp.Length != 6)
        {
            Message = "Enter all 6 digits.";
            _otpEntries.FirstOrDefault(entry => string.IsNullOrEmpty(entry.Text))?.Focus();
            return;
        }

        try
        {
            IsVerifying = true;
            Message = string.Empty;
            await Task.Delay(550);

            if (Otp != "123456")
            {
                Message = "Incorrect demo OTP. Use 123456.";
                return;
            }

            await DisplayAlert("Verified", "Demo OTP verified successfully.", "Continue");
            await Shell.Current.GoToAsync("//login");
        }
        finally
        {
            IsVerifying = false;
        }
    }

    protected override void OnDisappearing()
    {
        _countdownCts?.Cancel();
        _countdownCts?.Dispose();
        _countdownCts = null;
        base.OnDisappearing();
    }
}
