using System.Security.Cryptography;
using System.Text;

namespace ThogaiPay.Services.Authentication;

/// <summary>
/// Stores an expensive, salted verifier in SecureStorage.  A verifier is used instead of a raw PIN,
/// and failed attempts are throttled because a four digit PIN has a small search space.
/// </summary>
public sealed class SecurePinAuthenticationService : IPinAuthenticationService
{
    private const string PinVerifierKey = "thogaipay.pin.verifier.v1";
    private const string FailedAttemptsKey = "thogaipay.pin.failed-attempts";
    private const string LockedUntilKey = "thogaipay.pin.locked-until";
    private const int Iterations = 210_000;

    public async Task<bool> HasPinAsync() => !string.IsNullOrWhiteSpace(await SecureStorage.Default.GetAsync(PinVerifierKey));

    public async Task<PinOperationResult> SetPinAsync(string pin, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        if (!IsValidPin(pin)) return new(false, "Choose a 4-digit PIN.");

        var salt = RandomNumberGenerator.GetBytes(16);
        var hash = Rfc2898DeriveBytes.Pbkdf2(Encoding.UTF8.GetBytes(pin), salt, Iterations, HashAlgorithmName.SHA256, 32);
        await SecureStorage.Default.SetAsync(PinVerifierKey, $"v1:{Iterations}:{Convert.ToBase64String(salt)}:{Convert.ToBase64String(hash)}");
        Preferences.Default.Remove(FailedAttemptsKey);
        Preferences.Default.Remove(LockedUntilKey);
        return new(true);
    }

    public async Task<PinOperationResult> VerifyPinAsync(string pin, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var lockedUntil = Preferences.Default.Get(LockedUntilKey, 0L);
        if (lockedUntil > DateTimeOffset.UtcNow.ToUnixTimeSeconds())
        {
            var remaining = TimeSpan.FromSeconds(lockedUntil - DateTimeOffset.UtcNow.ToUnixTimeSeconds()).Seconds + 1;
            return new(false, $"Too many attempts. Try again in {remaining} seconds.");
        }

        var encoded = await SecureStorage.Default.GetAsync(PinVerifierKey);
        if (!IsValidPin(pin) || string.IsNullOrWhiteSpace(encoded) || !TryRead(encoded, out var salt, out var expected))
            return new(false, "PIN is not available. Verify your mobile number again.");

        var actual = Rfc2898DeriveBytes.Pbkdf2(Encoding.UTF8.GetBytes(pin), salt, Iterations, HashAlgorithmName.SHA256, expected.Length);
        if (CryptographicOperations.FixedTimeEquals(actual, expected))
        {
            Preferences.Default.Remove(FailedAttemptsKey);
            Preferences.Default.Remove(LockedUntilKey);
            return new(true);
        }

        var failures = Preferences.Default.Get(FailedAttemptsKey, 0) + 1;
        Preferences.Default.Set(FailedAttemptsKey, failures);
        if (failures >= 5)
        {
            Preferences.Default.Set(LockedUntilKey, DateTimeOffset.UtcNow.AddMinutes(1).ToUnixTimeSeconds());
            Preferences.Default.Set(FailedAttemptsKey, 0);
            return new(false, "Too many attempts. Try again in 1 minute.");
        }
        return new(false, "Incorrect PIN. Please try again.");
    }

    public Task ClearPinAsync()
    {
        SecureStorage.Default.Remove(PinVerifierKey);
        Preferences.Default.Remove(FailedAttemptsKey);
        Preferences.Default.Remove(LockedUntilKey);
        return Task.CompletedTask;
    }

    private static bool IsValidPin(string pin) => pin?.Length == 4 && pin.All(char.IsDigit);
    private static bool TryRead(string value, out byte[] salt, out byte[] hash)
    {
        salt = Array.Empty<byte>(); hash = Array.Empty<byte>();
        var parts = value.Split(':');
        if (parts.Length != 4 || parts[0] != "v1" || !int.TryParse(parts[1], out var iterations) || iterations != Iterations) return false;
        try { salt = Convert.FromBase64String(parts[2]); hash = Convert.FromBase64String(parts[3]); return salt.Length == 16 && hash.Length == 32; }
        catch (FormatException) { return false; }
    }
}
