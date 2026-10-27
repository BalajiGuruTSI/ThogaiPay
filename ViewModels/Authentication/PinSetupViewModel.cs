using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using ThogaiPay.Services.Authentication;

namespace ThogaiPay.ViewModels.Authentication;

public sealed class PinSetupViewModel : INotifyPropertyChanged
{
    private readonly IPinAuthenticationService _pins;
    private string _pin = string.Empty, _confirmation = string.Empty, _error = string.Empty;
    private bool _busy;
    public event PropertyChangedEventHandler? PropertyChanged;
    public ICommand ContinueCommand { get; }
    public string Pin { get => _pin; set => Set(ref _pin, Digits(value)); }
    public string Confirmation { get => _confirmation; set => Set(ref _confirmation, Digits(value)); }
    public string ErrorMessage { get => _error; set => Set(ref _error, value); }
    public bool IsBusy { get => _busy; set => Set(ref _busy, value); }

    public PinSetupViewModel(IPinAuthenticationService pins)
    {
        _pins = pins;
        ContinueCommand = new Command(async () => await ContinueAsync());
    }

    private async Task ContinueAsync()
    {
        if (IsBusy) return;
        ErrorMessage = string.Empty;
        if (Pin.Length != 4 || Confirmation.Length != 4) { ErrorMessage = "Enter and confirm your 4-digit PIN."; return; }
        if (!string.Equals(Pin, Confirmation, StringComparison.Ordinal)) { ErrorMessage = "PINs do not match."; return; }
        try
        {
            IsBusy = true;
            var result = await _pins.SetPinAsync(Pin);
            Pin = Confirmation = string.Empty;
            if (!result.IsSuccess) { ErrorMessage = result.Message ?? "Unable to set PIN."; return; }
            await Shell.Current.GoToAsync(nameof(Views.Authentication.BiometricSetupPage));
        }
        catch { ErrorMessage = "Your PIN could not be stored securely on this device."; }
        finally { IsBusy = false; }
    }
    private static string Digits(string? value) => new((value ?? string.Empty).Where(char.IsDigit).Take(4).ToArray());
    private void Set<T>(ref T field, T value, [CallerMemberName] string? name = null) { if (EqualityComparer<T>.Default.Equals(field, value)) return; field = value; PropertyChanged?.Invoke(this, new(name)); }
}
