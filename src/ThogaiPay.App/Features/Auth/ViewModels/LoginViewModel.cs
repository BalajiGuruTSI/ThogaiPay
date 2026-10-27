using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using ThogaiPay.App.Shared.Localization;

namespace ThogaiPay.App.Features.Auth.ViewModels;

/// <summary>
/// Login presentation state. Language is supplied by the app-wide localization service.
/// Demo OTP flow: accepts static OTP 123456; no SMS is sent.
/// </summary>
public sealed class LoginViewModel : INotifyPropertyChanged, IDisposable
{
    private readonly ILocalizationService _localization;
    private string _mobileNumber = string.Empty;
    private string _message = string.Empty;
    private bool _isMessageError;
    private bool _isContinuing;

    public event PropertyChangedEventHandler? PropertyChanged;

    public LoginViewModel(ILocalizationService localization)
    {
        _localization = localization;
        _localization.LanguageChanged += OnLanguageChanged;
        ContinueCommand = new Command(async () => await OnContinueAsync(), () => !IsContinuing);
        CreateAccountCommand = new Command(() =>
        {
            IsMessageError = false;
            Message = T("CreateAccountPending");
        });
    }

    private string T(string key) => _localization.GetString(key);

    public string MobileNumber
    {
        get => _mobileNumber;
        set
        {
            var normalized = new string((value ?? string.Empty).Where(char.IsDigit).Take(10).ToArray());
            if (_mobileNumber == normalized) return;
            _mobileNumber = normalized;
            OnPropertyChanged();
            if (!string.IsNullOrEmpty(Message)) Message = string.Empty;
        }
    }

    public bool IsContinuing
    {
        get => _isContinuing;
        private set
        {
            if (_isContinuing == value) return;
            _isContinuing = value;
            OnPropertyChanged();
            (ContinueCommand as Command)?.ChangeCanExecute();
        }
    }

    public string WelcomeTitle => T("WelcomeTitle");
    public string LoginSubtitle => T("LoginSubtitle");
    public string MobileFieldLabel => T("MobileFieldLabel");
    public string MobilePlaceholder => T("MobilePlaceholder");
    public string ContinueText => T("ContinueText");
    public string ContinueBusyText => T("ContinueBusyText");
    public string OrText => T("OrText");
    public string CreateAccountText => T("CreateAccountText");
    public string TrustText => T("TrustText");

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

    public ICommand ContinueCommand { get; }
    public ICommand CreateAccountCommand { get; }

    private async Task OnContinueAsync()
    {
        if (IsContinuing)
            return;

        if (MobileNumber.Length != 10 || MobileNumber[0] is < '6' or > '9')
        {
            IsMessageError = true;
            Message = T("InvalidMobile");
            return;
        }

        IsMessageError = false;
        Message = string.Empty;

        try
        {
            IsContinuing = true;
            // Brief, deliberate in-button progress feedback for the demo flow.
            await Task.Delay(650);

            // Demo-only OTP flow. Static OTP is 123456; no SMS is sent.
            await Shell.Current.GoToAsync($"otp?mobileNumber={Uri.EscapeDataString(MobileNumber)}");
        }
        catch (Exception ex)
        {
            IsMessageError = true;
            Message = $"Unable to open OTP verification: {ex.Message}";
        }
        finally
        {
            IsContinuing = false;
        }
    }

    private void OnLanguageChanged(object? sender, EventArgs e)
    {
        foreach (var property in new[] { nameof(WelcomeTitle), nameof(LoginSubtitle), nameof(MobileFieldLabel),
                     nameof(MobilePlaceholder), nameof(ContinueText), nameof(ContinueBusyText), nameof(OrText),
                     nameof(CreateAccountText), nameof(TrustText), nameof(Message) })
            OnPropertyChanged(property);

        if (!string.IsNullOrWhiteSpace(Message))
            Message = T(IsMessageError ? "InvalidMobile" : "ValidMobilePending");
    }

    public void Dispose() => _localization.LanguageChanged -= OnLanguageChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
