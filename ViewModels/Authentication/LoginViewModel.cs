
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using ThogaiPay.Services.Authentication;
using ThogaiPay.Services.Configuration;

namespace ThogaiPay.ViewModels.Authentication;

public class LoginViewModel : INotifyPropertyChanged
{
    private readonly IAuthenticationService _authenticationService;
    private readonly ILocalizationService _localization;

    private string _mobileNumber = string.Empty;
    private bool _isBusy;
    private string? _errorMessage;
    private string? _helperMessage;

    public event PropertyChangedEventHandler? PropertyChanged;

    public ICommand SendOtpCommand { get; }

    public ICommand LoginCommand => SendOtpCommand;

    public IReadOnlyList<string> LanguageOptions { get; } =
        new[] { "English", "தமிழ்" };

    public Color EnglishLanguageBackground =>
    SelectedLanguage == "English"
        ? Color.FromArgb("#0759D8")
        : Colors.Transparent;

    public Color TamilLanguageBackground =>
        SelectedLanguage == "தமிழ்"
            ? Color.FromArgb("#0759D8")
            : Colors.Transparent;

    public Color EnglishLanguageTextColor =>
        SelectedLanguage == "English"
            ? Colors.White
            : Color.FromArgb("#526482");

    public Color TamilLanguageTextColor =>
        SelectedLanguage == "தமிழ்"
            ? Colors.White
            : Color.FromArgb("#526482");

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
    public string SelectedLanguage
    {
        get => _localization.CurrentLanguage == "ta"
            ? "தமிழ்"
            : "English";

        set
        {
            if ( string.IsNullOrWhiteSpace(value) )
                return;

            var languageCode = value == "தமிழ்" || value == "Tamil"
                ? "ta"
                : "en";

            if ( _localization.CurrentLanguage == languageCode )
            {
                // Refresh selection colors even when the language is unchanged.
                RefreshLanguageSelection();
                return;
            }

            _localization.CurrentLanguage = languageCode;

            OnPropertyChanged();
            OnPropertyChanged(nameof(LoginTitle));
            OnPropertyChanged(nameof(LoginSubtitle));
            OnPropertyChanged(nameof(MobileLabel));
            OnPropertyChanged(nameof(MobilePlaceholder));
            OnPropertyChanged(nameof(ContinueText));
            OnPropertyChanged(nameof(SafetyText));

            RefreshLanguageSelection();
        }
    }

    private void RefreshLanguageSelection()
    {
        OnPropertyChanged(nameof(EnglishLanguageBackground));
        OnPropertyChanged(nameof(TamilLanguageBackground));
        OnPropertyChanged(nameof(EnglishLanguageTextColor));
        OnPropertyChanged(nameof(TamilLanguageTextColor));
    }
   
    public string LoginTitle =>
        Local(
            "Welcome to ThogaiPay",
            "ThogaiPay-க்கு வரவேற்கிறோம்");

    public string LoginSubtitle =>
        Local(
            "Enter your mobile number to continue. We'll verify it with a one-time password.",
            "தொடர உங்கள் மொபைல் எண்ணை உள்ளிடுங்கள். OTP மூலம் சரிபார்ப்போம்.");

    public string MobileLabel =>
        Local("MOBILE NUMBER", "மொபைல் எண்");

    public string MobilePlaceholder =>
        Local("10-digit mobile number", "10 இலக்க மொபைல் எண்");

    public string ContinueText =>
        Local("Continue", "தொடருங்கள்");

    public string SafetyText =>
        Local(
            "Your number is safe with us",
            "உங்கள் மொபைல் எண் பாதுகாப்பாக உள்ளது");

    public LoginViewModel(
        IAuthenticationService authenticationService,
        ILocalizationService localization)
    {
        _authenticationService = authenticationService;
        _localization = localization;

        SendOtpCommand = new Command(
            async () => await SendOtpAsync());
    }

    private string Local(string english, string tamil)
    {
        return _localization.CurrentLanguage == "ta"
            ? tamil
            : english;
    }

    private void OnPropertyChanged(
        [CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(
            this,
            new PropertyChangedEventArgs(propertyName));
    }

    private async Task SendOtpAsync()
    {
        if ( IsBusy )
            return;

        ErrorMessage = null;
        HelperMessage = null;

        if ( !IsValidIndianMobile(MobileNumber) )
        {
            ErrorMessage = Local(
                "Enter a valid 10-digit mobile number.",
                "சரியான 10 இலக்க மொபைல் எண்ணை உள்ளிடவும்.");

            return;
        }

        try
        {
            IsBusy = true;

            var normalized = new string(
                MobileNumber.Where(char.IsDigit).ToArray());

            if ( normalized.Length == 12 &&
                normalized.StartsWith(
                    "91",
                    StringComparison.Ordinal) )
            {
                normalized = normalized[2..];
            }

            if ( normalized.Length > 10 )
                normalized = normalized[^10..];

            var response = await _authenticationService.LoginAsync(
                normalized);

            if ( !response.IsSuccess ||
                string.IsNullOrWhiteSpace(response.SessionId) )
            {
                ErrorMessage = response.Message ??
                    Local(
                        "Unable to send OTP. Please try again.",
                        "OTP அனுப்ப முடியவில்லை. மீண்டும் முயற்சிக்கவும்.");

                return;
            }

            HelperMessage = response.Message;

            await Shell.Current.GoToAsync(
                nameof(Views.Authentication.OtpVerificationPage),
                new Dictionary<string, object>
                {
                    ["sessionId"] = response.SessionId,
                    ["mobileNumber"] = normalized
                });
        }
        catch ( Exception ex )
        {
            System.Diagnostics.Debug.WriteLine(
                $"Login / OTP request failed: {ex}");

            ErrorMessage = Local(
                "Something went wrong while sending OTP.",
                "OTP அனுப்பும்போது பிழை ஏற்பட்டது. மீண்டும் முயற்சிக்கவும்.");
        }
        finally
        {
            IsBusy = false;
        }
    }

    private static bool IsValidIndianMobile(string? value)
    {
        if ( string.IsNullOrWhiteSpace(value) )
            return false;

        var digits = new string(
            value.Where(char.IsDigit).ToArray());

        if ( digits.Length == 12 &&
            digits.StartsWith(
                "91",
                StringComparison.Ordinal) )
        {
            digits = digits[2..];
        }

        return digits.Length == 10;
    }

    private void SetProperty<T>(
        ref T field,
        T value,
        [CallerMemberName] string? propertyName = null)
    {
        if ( EqualityComparer<T>.Default.Equals(field, value) )
            return;

        field = value;
        OnPropertyChanged(propertyName);
    }
}
