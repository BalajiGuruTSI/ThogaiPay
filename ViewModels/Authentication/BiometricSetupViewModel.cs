using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using ThogaiPay.Services.Authentication;

namespace ThogaiPay.ViewModels.Authentication;

public sealed class BiometricSetupViewModel : INotifyPropertyChanged
{
    private readonly IBiometricAuthenticationService _biometrics;
    private string _error = string.Empty;
    private bool _busy;
    public event PropertyChangedEventHandler? PropertyChanged;
    public ICommand EnableCommand { get; }
    public ICommand SkipCommand { get; }
    public bool IsAvailable => _biometrics.IsAvailable;
    public string MethodName => _biometrics.AvailableMethodName;
    public string ErrorMessage { get => _error; set => Set(ref _error, value); }
    public bool IsBusy { get => _busy; set => Set(ref _busy, value); }
    public BiometricSetupViewModel(IBiometricAuthenticationService biometrics) { _biometrics = biometrics; EnableCommand = new Command(async () => await EnableAsync()); SkipCommand = new Command(async () => await FinishAsync()); }
    private async Task EnableAsync() { if (IsBusy) return; IsBusy = true; ErrorMessage = string.Empty; try { if (await _biometrics.AuthenticateAsync("Enable ThogaiPay unlock")) { _biometrics.SetEnabled(true); await FinishAsync(); } else ErrorMessage = "Biometric authentication was cancelled or is unavailable."; } finally { IsBusy = false; } }
    private Task FinishAsync() => Shell.Current.GoToAsync("//MainTabs/MainPage");
    private void Set<T>(ref T field, T value, [CallerMemberName] string? name = null) { if (EqualityComparer<T>.Default.Equals(field, value)) return; field = value; PropertyChanged?.Invoke(this, new(name)); }
}
