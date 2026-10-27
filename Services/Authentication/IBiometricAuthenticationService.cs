namespace ThogaiPay.Services.Authentication;

public interface IBiometricAuthenticationService
{
    bool IsAvailable { get; }
    string AvailableMethodName { get; }
    bool IsEnabled { get; }
    Task<bool> AuthenticateAsync(string reason, CancellationToken cancellationToken = default);
    void SetEnabled(bool enabled);
}
