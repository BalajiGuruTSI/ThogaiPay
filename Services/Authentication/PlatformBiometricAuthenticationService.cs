namespace ThogaiPay.Services.Authentication;

/// <summary>Safe default adapter. Platform projects can replace this with Android BiometricPrompt / iOS LocalAuthentication.</summary>
public sealed class PlatformBiometricAuthenticationService : IBiometricAuthenticationService
{
    private const string EnabledKey = "thogaipay.biometric.enabled";
    public bool IsAvailable => false;
    public string AvailableMethodName => "Biometric authentication";
    public bool IsEnabled => IsAvailable && Preferences.Default.Get(EnabledKey, false);
    public Task<bool> AuthenticateAsync(string reason, CancellationToken cancellationToken = default) => Task.FromResult(false);
    public void SetEnabled(bool enabled) => Preferences.Default.Set(EnabledKey, enabled && IsAvailable);
}
