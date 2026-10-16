using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using ThogaiPay.Services.Authentication;

namespace ThogaiPay.ViewModels.Authentication;

public class LoginViewModel : INotifyPropertyChanged
{
    private readonly IAuthenticationService _authenticationService;
    private string _mobileNumber = string.Empty;
    private bool _isBusy;
    private string? _errorMessage;
    private string? _helperMessage;

    public event PropertyChangedEventHandler? PropertyChanged;

    public ICommand SendOtpCommand { get; }
    public ICommand LoginCommand => SendOtpCommand;

    public string MobileNumber
    {
        get => _mobileNumber;
        set => SetProperty(ref _mobileNumber, value);
    }

    public bool IsBusy
    {
        get => _isBusy;
        set => SetProperty(ref _isBusy, value);
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

    public LoginViewModel(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
        SendOtpCommand = new Command(async () => await SendOtpAsync());
    }

    private async Task SendOtpAsync()
    {
        if (IsBusy)
        {
            return;
        }

        ErrorMessage = null;
        HelperMessage = null;

        if (!IsValidIndianMobile(MobileNumber))
        {
            ErrorMessage = "Enter a valid 10-digit mobile number.";
            return;
        }

        try
        {
            IsBusy = true;
            var response = await _authenticationService.LoginAsync(MobileNumber);

            if (!response.IsSuccess || string.IsNullOrWhiteSpace(response.SessionId))
            {
                ErrorMessage = response.Message ?? "Unable to send OTP. Please try again.";
                return;
            }

            HelperMessage = response.Message;

            var normalized = new string(MobileNumber.Where(char.IsDigit).ToArray());
            if (normalized.Length > 10)
            {
                normalized = normalized[^10..];
            }

            await Shell.Current.GoToAsync(nameof(Views.Authentication.OtpVerificationPage), new Dictionary<string, object>
            {
                ["sessionId"] = response.SessionId,
                ["mobileNumber"] = normalized
            });
        }
        catch
        {
            ErrorMessage = "Something went wrong while sending OTP.";
        }
        finally
        {
            IsBusy = false;
        }
    }

    private static bool IsValidIndianMobile(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return false;
        }

        var digits = new string(value.Where(char.IsDigit).ToArray());
        if (digits.Length == 12 && digits.StartsWith("91", StringComparison.Ordinal))
        {
            digits = digits[2..];
        }

        return digits.Length == 10;
    }

    private void SetProperty<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value))
        {
            return;
        }

        field = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
