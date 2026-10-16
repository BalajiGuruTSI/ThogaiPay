using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using ThogaiPay.Services.Authentication;

namespace ThogaiPay.ViewModels.Authentication;

public class OtpVerificationViewModel : INotifyPropertyChanged
{
    private readonly IAuthenticationService _authenticationService;
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
            {
                OnPropertyChanged(nameof(MaskedMobileNumber));
            }
        }
    }

    public string Otp
    {
        get => _otp;
        set
        {
            if (SetProperty(ref _otp, value))
            {
                OnPropertyChanged(nameof(OtpDigit1));
                OnPropertyChanged(nameof(OtpDigit2));
                OnPropertyChanged(nameof(OtpDigit3));
                OnPropertyChanged(nameof(OtpDigit4));
                OnPropertyChanged(nameof(OtpDigit5));
                OnPropertyChanged(nameof(OtpDigit6));
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
        set => SetProperty(ref _errorMessage, value);
    }

    public string? HelperMessage
    {
        get => _helperMessage;
        set => SetProperty(ref _helperMessage, value);
    }

    public int ResendSecondsRemaining
    {
        get => _resendSecondsRemaining;
        set
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

    public string ResendStatusText => $"Resend in 00:{ResendSecondsRemaining:D2}";

    public string MaskedMobileNumber => MobileNumber.Length == 10
        ? $"+91 ••••••{MobileNumber[^4..]}"
        : "+91 ••••••••••";

    public string OtpDigit1 => Otp.Length > 0 ? Otp[0].ToString() : string.Empty;
    public string OtpDigit2 => Otp.Length > 1 ? Otp[1].ToString() : string.Empty;
    public string OtpDigit3 => Otp.Length > 2 ? Otp[2].ToString() : string.Empty;
    public string OtpDigit4 => Otp.Length > 3 ? Otp[3].ToString() : string.Empty;
    public string OtpDigit5 => Otp.Length > 4 ? Otp[4].ToString() : string.Empty;
    public string OtpDigit6 => Otp.Length > 5 ? Otp[5].ToString() : string.Empty;

    private readonly Command _resendOtpCommand;

    public OtpVerificationViewModel(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
        VerifyCommand = new Command(async () => await VerifyAsync());
        _resendOtpCommand = new Command(async () => await ResendOtpAsync(), () => CanResend);
        ResendOtpCommand = _resendOtpCommand;
        EditNumberCommand = new Command(async () => await Shell.Current.GoToAsync(".."));
        FocusOtpInputCommand = new Command(() => { });
    }

    public void Initialize(string session, string mobile)
    {
        SessionId = session;
        MobileNumber = new string((mobile ?? string.Empty).Where(char.IsDigit).ToArray());
        if (MobileNumber.Length > 10)
        {
            MobileNumber = MobileNumber[^10..];
        }

        StartResendTimer();
    }

    public void StopTimer()
    {
        _timerCts?.Cancel();
        _timerCts?.Dispose();
        _timerCts = null;
    }

    private void StartResendTimer()
    {
        StopTimer();
        _timerCts = new CancellationTokenSource();
        _ = RunResendCountdownAsync(_timerCts.Token);
    }

    private async Task RunResendCountdownAsync(CancellationToken cancellationToken)
    {
        ResendSecondsRemaining = 30;

        while (ResendSecondsRemaining > 0 && !cancellationToken.IsCancellationRequested)
        {
            await Task.Delay(1000, cancellationToken);

            if (!cancellationToken.IsCancellationRequested)
            {
                ResendSecondsRemaining--;
            }
        }
    }

    private async Task VerifyAsync()
    {
        if (IsBusy)
        {
            return;
        }

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
            ErrorMessage = "Session expired. Please resend OTP.";
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

            await Shell.Current.GoToAsync("//MainPage");
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
        {
            return;
        }

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

    private bool SetProperty<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value))
        {
            return false;
        }

        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
