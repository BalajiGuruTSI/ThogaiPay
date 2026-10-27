using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using ThogaiPay.Services.Authentication;

namespace ThogaiPay.ViewModels.Authentication;

public sealed class PinUnlockViewModel : INotifyPropertyChanged
{
    private readonly IPinAuthenticationService _pins;
    private string _pin = string.Empty, _error = string.Empty;
    private bool _busy;
    public event PropertyChangedEventHandler? PropertyChanged;
    public ICommand UnlockCommand { get; }
    public ICommand ForgotPinCommand { get; }
    public string Pin { get => _pin; set => Set(ref _pin, new string((value ?? string.Empty).Where(char.IsDigit).Take(4).ToArray())); }
    public string ErrorMessage { get => _error; set => Set(ref _error, value); }
    public bool IsBusy { get => _busy; set => Set(ref _busy, value); }
    public PinUnlockViewModel(IPinAuthenticationService pins) { _pins = pins; UnlockCommand = new Command(async () => await UnlockAsync()); ForgotPinCommand = new Command(async () => await Shell.Current.GoToAsync("//LoginPage")); }
    private async Task UnlockAsync()
    {
        if (IsBusy) return; ErrorMessage = string.Empty;
        try { IsBusy = true; var result = await _pins.VerifyPinAsync(Pin); Pin = string.Empty; if (!result.IsSuccess) { ErrorMessage = result.Message ?? "Unable to unlock."; return; } await Shell.Current.GoToAsync("//MainTabs/MainPage"); }
        catch { ErrorMessage = "Unable to unlock right now."; } finally { IsBusy = false; }
    }
    private void Set<T>(ref T field, T value, [CallerMemberName] string? name = null) { if (EqualityComparer<T>.Default.Equals(field, value)) return; field = value; PropertyChanged?.Invoke(this, new(name)); }
}
