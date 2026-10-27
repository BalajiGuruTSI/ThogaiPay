using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace ThogaiPay.App.ViewModels.Authentication;

/// <summary>
/// Presentation state and basic input validation for the login screen.
/// OTP delivery is intentionally not simulated; connect this command to the approved API use case.
/// </summary>
public sealed class LoginViewModel : INotifyPropertyChanged
{
    private string _mobileNumber = string.Empty;
    private string _message = string.Empty;
    private bool _isMessageError;
    private string _selectedLanguage = "English";

    public event PropertyChangedEventHandler? PropertyChanged;

    public LoginViewModel()
    {
        ContinueCommand = new Command(OnContinue);
        CreateAccountCommand = new Command(() =>
        {
            Message = SelectedLanguage == "தமிழ்" ? "பதிவு வசதி இணைக்கப்பட்ட பிறகு புதிய கணக்கு உருவாக்கலாம்." : "Account creation will be available when the registration flow is connected.";
            IsMessageError = false;
        });
        LanguageChangedCommand = new Command<string?>(language =>
        {
            if (!string.IsNullOrWhiteSpace(language))
                SelectedLanguage = language;
        });
    }

    public string MobileNumber
    {
        get => _mobileNumber;
        set
        {
            if (_mobileNumber == value) return;
            _mobileNumber = new string((value ?? string.Empty).Where(char.IsDigit).Take(10).ToArray());
            OnPropertyChanged();
            if (!string.IsNullOrEmpty(Message)) Message = string.Empty;
        }
    }

    public string Message
    {
        get => _message;
        private set { if (_message == value) return; _message = value; OnPropertyChanged(); OnPropertyChanged(nameof(HasMessage)); }
    }

    public bool HasMessage => !string.IsNullOrWhiteSpace(Message);
    public Color MessageColor => IsMessageError ? Color.FromArgb("#DC2626") : Color.FromArgb("#5C6680");

    public bool IsMessageError
    {
        get => _isMessageError;
        private set { if (_isMessageError == value) return; _isMessageError = value; OnPropertyChanged(); OnPropertyChanged(nameof(MessageColor)); }
    }

    public string SelectedLanguage
    {
        get => _selectedLanguage;
        set
        {
            if (_selectedLanguage == value) return;
            _selectedLanguage = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(WelcomeTitle));
            OnPropertyChanged(nameof(LoginSubtitle));
            OnPropertyChanged(nameof(MobilePlaceholder));
            OnPropertyChanged(nameof(ContinueText));
            OnPropertyChanged(nameof(OrText));
            OnPropertyChanged(nameof(CreateAccountText));
            OnPropertyChanged(nameof(TrustText));
        }
    }

    public string WelcomeTitle => SelectedLanguage == "தமிழ்" ? "மீண்டும் வரவேற்கிறோம்" : "Welcome Back";
    public string LoginSubtitle => SelectedLanguage == "தமிழ்" ? "உங்கள் ThogaiPay கணக்கில் உள்நுழையவும்" : "Login to your ThogaiPay account";
    public string MobilePlaceholder => SelectedLanguage == "தமிழ்" ? "மொபைல் எண்ணை உள்ளிடவும்" : "Enter mobile number";
    public string ContinueText => SelectedLanguage == "தமிழ்" ? "தொடரவும்  →" : "Continue  →";
    public string OrText => SelectedLanguage == "தமிழ்" ? "அல்லது" : "or";
    public string CreateAccountText => SelectedLanguage == "தமிழ்" ? "புதிய கணக்கை உருவாக்கவும்" : "Create New Account";
    public string TrustText => SelectedLanguage == "தமிழ்" ? "பாதுகாப்பானது மற்றும் நம்பகமானது" : "Secure & Trusted";

    public ICommand ContinueCommand { get; }
    public ICommand CreateAccountCommand { get; }
    public ICommand LanguageChangedCommand { get; }

    private void OnContinue()
    {
        if (MobileNumber.Length != 10 || MobileNumber[0] is < '6' or > '9')
        {
            IsMessageError = true;
            Message = SelectedLanguage == "தமிழ்" ? "சரியான 10 இலக்க மொபைல் எண்ணை உள்ளிடவும்." : "Enter a valid 10-digit Indian mobile number.";
            return;
        }

        // Do not claim that an OTP was sent until a real authentication API is integrated.
        IsMessageError = false;
        Message = SelectedLanguage == "தமிழ்" ? "மொபைல் எண் சரியானது. அங்கீகார API இணைக்கப்பட்ட பிறகு OTP அனுப்புதல் செயல்படும்." : "Your number is valid. OTP delivery will be enabled when the authentication API is connected.";
    }

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
