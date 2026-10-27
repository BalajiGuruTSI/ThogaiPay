using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Microsoft.Maui.ApplicationModel;
using ThogaiPay.Services.Authentication;

namespace ThogaiPay.ViewModels.Authentication;

public class OtpVerificationViewModel : INotifyPropertyChanged
{
    private readonly IAuthenticationService _authenticationService;
    private readonly IPinAuthenticationService _pinAuthenticationService;

    private CancellationTokenSource? _timerCts;
    private string _sessionId = string.Empty;
    private string _mobileNumber = string.Empty;
    private string _otp = string.Empty;
    private bool _isVerifying;
    private bool _isResending;
    private string? _errorMessage;
    private string? _helperMessage;
    private int _resendSecondsRemaining = 30;

    public event PropertyChangedEventHandler? PropertyChanged;

    public ICommand VerifyCommand { get; }
    public ICommand ResendOtpCommand { get; }
    public ICommand EditNumberCommand { get; }
    public ICommand FocusOtpInputCommand { get; }

    private readonly Command _resendOtpCommand;

    public OtpVerificationViewModel(
        IAuthenticationService authenticationService,
        IPinAuthenticationService pinAuthenticationService)
    {
        _authenticationService = authenticationService;
        _pinAuthenticationService = pinAuthenticationService;

        VerifyCommand = new Command(async () => await VerifyAsync());
        _resendOtpCommand = new Command(async () => await ResendOtpAsync(), () => CanResend);
        ResendOtpCommand = _resendOtpCommand;
        EditNumberCommand = new Command(async () => await EditNumberAsync());
        FocusOtpInputCommand = new Command(() => { });
    }

    public string SessionId
    {
        get => _sessionId;
        set => SetProperty(ref _sessionId, value);
    }

    public string MobileNumber
    {
        get => _mobileNumber;
        set
        {
            if (SetProperty(ref _mobileNumber, value))
                OnPropertyChanged(nameof(MaskedMobileNumber));
        }
    }

    public string Otp
    {
        get => _otp;
        set
        {
            var normalized = new string((value ?? string.Empty)
                .Where(char.IsDigit)
                .Take(6)
                .ToArray());

            if (SetProperty(ref _otp, normalized))
            {
                OnPropertyChanged(nameof(OtpDigit1));
                OnPropertyChanged(nameof(OtpDigit2));
                OnPropertyChanged(nameof(OtpDigit3));
                OnPropertyChanged(nameof(OtpDigit4));
                OnPropertyChanged(nameof(OtpDigit5));
                OnPropertyChanged(nameof(OtpDigit6));
                ErrorMessage = null;
            }
        }
    }

    public bool IsBusy => IsVerifying || IsResending;

    public bool IsVerifying
    {
        get => _isVerifying;
        private set
        {
            if (SetProperty(ref _isVerifying, value))
            {
                OnPropertyChanged(nameof(IsBusy));
                OnPropertyChanged(nameof(CanResend));
                _resendOtpCommand.ChangeCanExecute();
            }
        }
    }

    public bool IsResending
    {
        get => _isResending;
        private set
        {
            if (SetProperty(ref _isResending, value))
            {
                OnPropertyChanged(nameof(IsBusy));
                OnPropertyChanged(nameof(CanResend));
                _resendOtpCommand.ChangeCanExecute();
            }
        }
    }

    public string? ErrorMessage
    {
        get => _errorMessage;
        set
        {
            if (SetProperty(ref _errorMessage, value))
                OnPropertyChanged(nameof(HasError));
        }
    }

    public string? HelperMessage
    {
        get => _helperMessage;
        set
        {
            if (SetProperty(ref _helperMessage, value))
                OnPropertyChanged(nameof(HasHelperMessage));
        }
    }

    public bool HasError => !string.IsNullOrWhiteSpace(ErrorMessage);
    public bool HasHelperMessage => !string.IsNullOrWhiteSpace(HelperMessage);

    public int ResendSecondsRemaining
    {
        get => _resendSecondsRemaining;
        private set
        {
            if (SetProperty(ref _resendSecondsRemaining, value))
            {
                OnPropertyChanged(nameof(CanResend));
                OnPropertyChanged(nameof(ResendStatusText));
                _resendOtpCommand.ChangeCanExecute();
            }
        }
    }

    public bool CanResend => ResendSecondsRemaining <= 0 && !IsBusy;

    public string ResendStatusText
    {
        get
        {
            var seconds = Math.Max(0, ResendSecondsRemaining);
            return $"Resend in 00:{seconds:D2}";
        }
    }

    public string MaskedMobileNumber => MobileNumber.Length == 10
        ? $"+91 ••••••{MobileNumber[^4..]}"
        : "+91 ••••••••••";

    public string OtpDigit1 => Otp.Length > 0 ? Otp[0].ToString() : string.Empty;
    public string OtpDigit2 => Otp.Length > 1 ? Otp[1].ToString() : string.Empty;
    public string OtpDigit3 => Otp.Length > 2 ? Otp[2].ToString() : string.Empty;
    public string OtpDigit4 => Otp.Length > 3 ? Otp[3].ToString() : string.Empty;
    public string OtpDigit5 => Otp.Length > 4 ? Otp[4].ToString() : string.Empty;
    public string OtpDigit6 => Otp.Length > 5 ? Otp[5].ToString() : string.Empty;

    public void Initialize(string session, string mobile)
    {
        SessionId = session ?? string.Empty;

        var digits = new string((mobile ?? string.Empty)
            .Where(char.IsDigit)
            .ToArray());

        MobileNumber = digits.Length > 10 ? digits[^10..] : digits;
        Otp = string.Empty;
        ErrorMessage = null;
        HelperMessage = null;
        StartResendTimer();
    }

    public void StopTimer()
    {
        var oldCts = _timerCts;
        _timerCts = null;

        if (oldCts is null)
            return;

        try
        {
            oldCts.Cancel();
        }
        finally
        {
            oldCts.Dispose();
        }
    }

    private void StartResendTimer()
    {
        StopTimer();

        _timerCts = new CancellationTokenSource();
        _ = RunResendCountdownAsync(_timerCts.Token);
    }

    private async Task RunResendCountdownAsync(CancellationToken cancellationToken)
    {
        await MainThread.InvokeOnMainThreadAsync(() => ResendSecondsRemaining = 30);

        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(1000, cancellationToken);

                if (cancellationToken.IsCancellationRequested)
                    break;

                await MainThread.InvokeOnMainThreadAsync(() =>
                {
                    if (ResendSecondsRemaining > 0)
                        ResendSecondsRemaining--;
                });

                if (ResendSecondsRemaining <= 0)
                    break;
            }
        }
        catch (OperationCanceledException)
        {
            // Expected when the page disappears or a new timer starts.
        }
    }

    private async Task EditNumberAsync()
    {
        try
        {
            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Unable to return to mobile number entry: {ex.Message}";
        }
    }

    private async Task VerifyAsync()
    {
        if (IsBusy)
            return;

        ErrorMessage = null;
        HelperMessage = null;

        var normalizedOtp = (Otp ?? string.Empty).Trim();

        if (normalizedOtp.Length != 6 || !normalizedOtp.All(char.IsDigit))
        {
            ErrorMessage = "Please enter a valid 6-digit OTP.";
            return;
        }

        if (string.IsNullOrWhiteSpace(SessionId))
        {
            ErrorMessage = "Session expired. Please request a new OTP.";
            return;
        }

        try
        {
            IsVerifying = true;

            var response = await _authenticationService.VerifyOtpAsync(SessionId, normalizedOtp);
            if (!response.IsSuccess)
            {
                ErrorMessage = response.Message ?? "OTP verification failed.";
                return;
            }

            if (Shell.Current is null)
            {
                ErrorMessage = "Unable to continue. Please restart the app.";
                return;
            }

            if (await _pinAuthenticationService.HasPinAsync())
                await Shell.Current.GoToAsync("//MainTabs/MainPage");
            else
                await Shell.Current.GoToAsync(nameof(Views.Authentication.PinSetupPage));
        }
        catch (Exception ex)
        {
            ErrorMessage = string.IsNullOrWhiteSpace(ex.Message)
                ? "Something went wrong during verification."
                : $"Verification failed: {ex.Message}";
        }
        finally
        {
            IsVerifying = false;
        }
    }

    private async Task ResendOtpAsync()
    {
        if (!CanResend || IsBusy)
            return;

        try
        {
            IsResending = true;
            ErrorMessage = null;
            HelperMessage = null;

            var response = await _authenticationService.LoginAsync(MobileNumber);

            if (!response.IsSuccess || string.IsNullOrWhiteSpace(response.SessionId))
            {
                ErrorMessage = response.Message ?? "Unable to resend OTP.";
                return;
            }

            SessionId = response.SessionId;
            HelperMessage = response.Message;
            Otp = string.Empty;
            StartResendTimer();
        }
        catch
        {
            ErrorMessage = "Unable to resend OTP right now.";
        }
        finally
        {
            IsResending = false;
        }
    }

    private bool SetProperty<T>(
        ref T field,
        T value,
        [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value))
            return false;

        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
